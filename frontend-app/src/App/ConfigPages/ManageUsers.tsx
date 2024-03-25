import React, { useEffect, useState, useCallback } from "react";
import { proxyURL, ADMIN, BASIC } from "../../variables.js";
import axios from "axios";
import { UserType, UserDeviceSelectedType, DeviceType, UserDeviceType } from "../../interfaces.js";

interface ManageUsersProps {
    manageUsers: ((isManagingUsers: boolean) => void);
    isManagingUsers: boolean;
    addUser: ((addingUser: boolean) => void);
    isAddingUser: boolean;
    updateUser: ((updatingUser: boolean) => void);
    isUpdatingUser: boolean;
}

var shajs = require('sha.js');

const ManageUsers: React.FC<ManageUsersProps>  = ({
    manageUsers,
    isManagingUsers,
    addUser,
    isAddingUser,
    updateUser,
    isUpdatingUser,
}: ManageUsersProps) => {

    const [selectedUserID, setSelectedUserID] = useState(0);
    const [selectedUser, setSelectedUser] = useState({} as UserType);
    const [users, setUsers] = useState([] as UserType[]);

    const [newUserFirstName, setNewUserFirstName] = useState('');
    const [newUserLastName, setNewUserLastName] = useState('');
    const [newUserType, setNewUserType] = useState('');

    const [updatePassword, setUpdatePassword] = useState(false);
    const [newUserPassword, setNewUserPassword] = useState('');
    const [oldUserPassword, setOldUserPassword] = useState('');

    const [isDeletingUser, setIsDeletingUser] = useState(false);

    const [updateUserAttempt, setUpdateUserAttempt] = useState(false);
    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);
    const [addUserAttempt, setAddUserAttempt] = useState(false);
    const [addedCorrectly, setAddedCorrectly] = useState(false);
    const [updateDeleteMessage, setUpdateDeleteMessage] = useState('');
    const [error, setError] = useState('');

    const [allUserDevices, setAllUserDevices] = useState([] as UserDeviceType[]);
    const [userDevices, setUserDevices] = useState([] as UserDeviceSelectedType[]);
    const [devices, setDevices] = useState([] as DeviceType[]);

    const [updatedUD, setUpdatedUD] = useState(false);

    const getDevices = useCallback(async() => {
        let tempAllDevices: DeviceType[] = [];
        let tempNonDeletedDevices: DeviceType[] = [];

        if((selectedUserID !== undefined && selectedUserID !== 0) || isAddingUser) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Device`,
            })
            .then(function (response) {
                tempAllDevices = response.data;
            }).catch(error => {
                console.log(error);
            })

            console.log('tempAllDevices', tempAllDevices)

            tempAllDevices.forEach((device) => {
                if(!device.deviceIsDeleted) {
                    tempNonDeletedDevices.push(device);
                }
            }) 

            console.log('tempNonDeletedDevices', tempNonDeletedDevices)
            setDevices(tempNonDeletedDevices);
        }
    }, [selectedUserID, isAddingUser]);

    const getUserDevices = useCallback(async() => {
        let tempUserDevices: UserDeviceType[] = [];
        let tempSelectedUserDevices: UserDeviceSelectedType[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/UserDevice`,
        })
            .then(function (response) {
                tempUserDevices = response.data;
                setAllUserDevices(tempUserDevices);

                devices.forEach((device, i) => {
                    let isSelected = false;
                    tempUserDevices.forEach((ud, j) => {
                        if((device.deviceID === ud.deviceID) && (selectedUserID === ud.userID)) {
                            isSelected = true;
                            return;
                        }
                    })

                    tempSelectedUserDevices.push({
                        deviceID: device.deviceID,
                        isSelected: isSelected
                    } as UserDeviceSelectedType);
                })

                setUserDevices(tempSelectedUserDevices);

            }).catch(error => {
                console.log(error);
            })

    }, [selectedUserID, devices]);

    useEffect(() => {
        setUpdatedUD(false);
    }, [updatedUD]);

    useEffect(() => {
        getUserDevices();
    }, [selectedUserID, isAddingUser, getUserDevices]);

    function selectUser(user: UserType) {
        setSelectedUser(user);
        setSelectedUserID(user.userID);
        setNewUserFirstName(user.userFirstName);
        setNewUserLastName(user.userLastName);
        setNewUserType(user.userType);
    }

    const getUsers = useCallback(async() => {
        let tempAllUsers: UserType[] = [];
        let tempNonDeletedUsers: UserType[] = [];

        if(isManagingUsers) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User`,
            })
            .then(function (response) {
                tempAllUsers = response.data;
            }).catch(error => {
                setError(error);
                console.log(error);
            })

            tempAllUsers.forEach((user) => {
                if(!user.userIsDeleted) {
                    tempNonDeletedUsers.push(user);
                }
            })

            setUsers(tempNonDeletedUsers);
        }
        
    }, [isManagingUsers]);

    async function postUserDevice(userID: number, deviceID: number): Promise<boolean> {
        let postedCorrectly = false;

        await axios.post(`${proxyURL}/api/UserDevice?userId=${userID}&deviceId=${deviceID}`, {
            deviceID: deviceID,
            userID: userID
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            postedCorrectly =  true;
        }).catch(function (error) {
            setError(error);
            console.log(error);
        });

        return Promise.resolve(postedCorrectly);
    }

    async function deleteUserDevice(userID: number, deviceID: number): Promise<boolean> {
        let deletedCorrectly = false;

        await axios.delete(`${proxyURL}/api/UserDevice/${userID}:${deviceID}`)
            .then(function (response) {
                deletedCorrectly =  true;
            }).catch(function (error) {
                console.log(error);
            });

        return Promise.resolve(deletedCorrectly);
    }

    async function updateUserInDB(deletedUser: boolean) {
        let postedCorrectly: Promise<boolean>;
        let deletedCorrectly: Promise<boolean>;

        let canUpdate = true;

        let tempUserPassword = updatePassword ? newUserPassword : selectedUser.userPassword;
        const hashedPassword = shajs('sha256').update(tempUserPassword).digest('hex');

        setUpdateUserAttempt(true);

        users.forEach((user) => {
            if(
                user.userFirstName === newUserFirstName &&
                user.userLastName === newUserLastName &&
                user.userPassword === hashedPassword &&
                user.userID !== selectedUserID
            ) {
                canUpdate = false;
                return;
            }
        });

        if(canUpdate) {
            // Update user in User Table
            await axios.put(`${proxyURL}/api/User/${selectedUserID}`, {
                userID: selectedUserID,
                userType: newUserType,
                userFirstName: newUserFirstName,
                userLastName: newUserLastName,
                userPassword: hashedPassword,
                userEmail: '',
                userPhone: '',
                userNotification: false,
                userIsDeleted: deletedUser,
            }, {
                headers: {
                    'Content-Type': 'application/json'
                    }
            })
            .then(function (response) {
                setUsers(response.data);
                setUpdatedCorrectly(true);
                setUpdateDeleteMessage(deletedUser 
                        ? 'Successfully Deleted User.' : 'Successfully Updated User.');
            }).catch(error => {
                console.log(error);
                setError(error);
            });

            if(!deletedUser) {
                // Update user devices in UserDevices Table
                if (newUserType.toUpperCase() === ADMIN) {
                    userDevices.forEach((ud, i) => {
                        let isFound = false;
                        allUserDevices.forEach((ud2) => {
                            if((selectedUserID === ud2.userID) && (ud.deviceID === ud2.deviceID)) {
                                isFound = true;
                                return;
                            }
                        })

                        if (!isFound) {
                            postedCorrectly = postUserDevice(selectedUserID, ud.deviceID);
                        }

                        if(!postedCorrectly || !deletedCorrectly) {
                            return;
                        }
                    })
                } 
                
                else {
                    userDevices.forEach((ud, i) => {
                        let isFound = false;
                        allUserDevices.forEach((ud2) => {
                            if((selectedUserID === ud2.userID) && (ud.deviceID === ud2.deviceID)) {
                                isFound = true;
                                return;
                            }
                        })
            
                        if(ud.isSelected && !isFound) {
                            postedCorrectly = postUserDevice(selectedUserID, ud.deviceID);
                        }
                        else if(!(ud.isSelected) && isFound) {
                            deletedCorrectly = deleteUserDevice(selectedUserID, ud.deviceID);
                        }
                        if(!postedCorrectly || !deletedCorrectly) {
                            return;
                        }
                    })
                }
            }
        } else {
            setUpdatedCorrectly(false);
            setError('New User Info Matches Another User.')
        }

        updateUser(false);
        resetState();
        getUsers();
    }

    async function addUserToDB() {
        setAddUserAttempt(true);
        let canAdd = true;
        const hashedPassword = shajs('sha256').update(newUserPassword).digest('hex');

        users.forEach((user) => {
            if(
                user.userFirstName === newUserFirstName &&
                user.userLastName === newUserLastName &&
                user.userPassword === hashedPassword
            ) {
                canAdd = false;
                return;
            }
        });

        if(canAdd) {
            await axios.post(`${proxyURL}/api/User`, {
                userType: newUserType,
                userFirstName: newUserFirstName,
                userLastName: newUserLastName,
                userPassword: hashedPassword,
                userEmail: '',
                userPhone: '',
                userNotification: false,
            }, {
                headers: {
                    'Content-Type': 'application/json'
                    }
            })
            .then(function (response) {
                setUsers(response.data);
                setAddedCorrectly(true);
            }).catch(error => {
                console.log(error);
                setError(error)
            });
    
            let tempUser: UserType;

            // Get User to then add their UserDevices
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User/${newUserFirstName}.${newUserLastName}:${hashedPassword}`,
            })
                .then(function (response) {
                    tempUser = response.data;

                    if(tempUser) {
                        let postedCorrectly: Promise<boolean>;
    
                        if (tempUser.userType.toUpperCase() === ADMIN) {
                            devices.forEach((device) => {
                                postedCorrectly = postUserDevice(tempUser.userID, device.deviceID);
                                if(!postedCorrectly) {
                                    return;
                                }
                            })
                        } 
                        
                        else {
                            userDevices.forEach((ud) => {
                                if(ud.isSelected) {
                                    postedCorrectly = postUserDevice(tempUser.userID, ud.deviceID);
                                }

                                if(!postedCorrectly) {
                                    return;
                                }
                            })
                        }
                    }
                }).catch(error => {
                    setError(error);
                    console.log(error);
                })
        } else {
            setAddedCorrectly(false);
            setError('New User Info Matches Another User.')
        }

        addUser(false);
        resetState();
        getUsers();
    }

    function resetState() {
        setSelectedUserID(0);
        setNewUserFirstName('');
        setNewUserLastName('');
        setNewUserType('');
        setUpdatePassword(false);
        setOldUserPassword('');
        setNewUserPassword('');
    }

    useEffect(() => {
        getUsers();
        getDevices();
    }, [getUsers, getDevices])

    return(
        <div className="ManageUsersMainBox">
            {
                updateUserAttempt ?
                    <div className="UserActionResultMainBox"> 
                        <div className="UserActionResultErrorText">
                            {updatedCorrectly ?  
                                <div className="UserActionResultErrorSubText">   
                                    <span >
                                        {updateDeleteMessage}
                                    </span>
                                </div> : 
                                <div className="UserActionResultErrorSubText">
                                    <span> { `${error}`} </span>
                                    <span> Unable to Update User. </span>
                                </div>                                
                            }
                        </div>
                        <span 
                            className="UserActionResultExitButton"
                            onClick={(e) => {
                                setUpdateUserAttempt(false);
                            }}
                        >
                            X
                        </span>
                    </div> 
                : addUserAttempt ?
                    <div className="UserActionResultMainBox"> 
                        <div className="UserActionResultErrorText">
                            {addedCorrectly ?  
                                <div className="UserActionResultErrorSubText">   
                                    <span >
                                        {`Successfully Added User`}
                                    </span>
                                </div> : 
                                <div className="UserActionResultErrorSubText">
                                    <span> { `${error}`} </span>
                                    <span> Unable to Add User </span>
                                </div>                                
                            }
                        </div>
                        <span 
                            className="UserActionResultExitButton"
                            onClick={(e) => {
                                setAddUserAttempt(false);
                            }}
                        >
                            X
                        </span>
                    </div> 
                :
                isUpdatingUser ?
                <div className="ManageUsersInfoSubBox">
                    <div className="UserIDTitleBox">
                        <div className="userTitleText">
                            {`User ID - ${selectedUserID}`}
                        </div>
                    </div>
                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            First Name
                        </div>
                        <textarea
                        className="manageUserTextArea"
                        value={newUserFirstName}
                        id={'FIRST_NAME'}
                        onChange={(e) => {setNewUserFirstName(e.target.value.toString());}}
                        spellCheck={false}
                        cols={1}
                        rows={1} >{newUserFirstName}</textarea>
                    </div>

                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            Last Name
                        </div>
                        <textarea
                        className="manageUserTextArea"
                        value={newUserLastName}
                        id={'LAST_NAME'}
                        onChange={(e) => {setNewUserLastName(e.target.value.toString());}}
                        spellCheck={false}
                        cols={1}
                        rows={1} >{newUserLastName}</textarea>
                    </div>

                    
                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            User Type
                        </div>
                        <div className="EditUserTypeSubBox">
                            <button
                                className={(newUserType.toUpperCase() === ADMIN) ? 
                                    "EditUserTypeButtonSelected" : "EditUserTypeButton"}
                                onClick={() => {
                                    setNewUserType(ADMIN);
                                }}
                                >
                                Admin 
                            </button>
                            
                            <button 
                                className={(newUserType.toUpperCase() === BASIC) ? 
                                    "EditUserTypeButtonSelected" : "EditUserTypeButton"}
                                onClick={() => {
                                    setNewUserType(BASIC);
                                }}
                                >
                                Basic
                            </button>
                        </div>
                    </div>
                    {
                        updatePassword ? null :
                        <div className="UserListMainBox">
                            <button 
                                className="UpdatePasswordButton"
                                onClick={(e) => {
                                        setUpdatePassword(true);
                                    }}
                                    > Update Password
                            </button>
                        </div>
                    }
                    {
                        updatePassword ?
                            <div className="UpdatingPasswordMainBox">
                                <div className="UpdatePasswordFieldsBox">
                                    <div className="UpdatingPasswordSubBox">
                                        <div className="UpdatePasswordText">
                                            Old Password
                                        </div>
                                        <input
                                            className="UpdatePasswordTextArea"
                                            value={oldUserPassword}
                                            id={'OLD_PASSWORD'}
                                            type="password"
                                            onChange={(e) => {setOldUserPassword(e.target.value.toString());}}
                                            >
                                        </input>
                                    </div>

                                    <div className="UpdatingPasswordSubBox">
                                        <div className="UpdatePasswordText">
                                            New Password
                                        </div>
                                        <input
                                            className="UpdatePasswordTextArea"
                                            value={newUserPassword}
                                            id={'NEW_PASSWORD'}
                                            type="password"
                                            onChange={(e) => {setNewUserPassword(e.target.value.toString());}}
                                            >
                                        </input>
                                    </div>
                                </div>    
                                <span 
                                    className="UpdatePasswordExitButton"
                                    onClick={(e) => {
                                        setUpdatePassword(false);
                                    }}
                                >
                                    X
                                </span> 
                            </div> 
                        : null
                    }
                    <div className="DeviceOptionMainBox">
                        <div className="DeviceOptionTitleText">
                            <div>Devices</div>
                            <span className={"DeviceOptionButtonLegend"}></span>
                            <span className={"DeviceOptionLegendEqualsText"}>=</span>
                            <span className={"DeviceOptionLegendSelectedText"}>Selected</span>
                        </div>
                        { 
                            devices.map((device, i) => {
                                let isSelected = false;
                                userDevices.forEach((ud, i) => {
                                    if((ud.deviceID === device.deviceID)) {
                                        isSelected = ud.isSelected;
                                    }
                                })
                                
                                return (
                                    <div className={(i === 0) ? "DeviceOptionTopSubBox" : "DeviceOptionSubBox"} key={i}>
                                        <span 
                                            className={
                                                (newUserType.toUpperCase() === ADMIN) ? "DeviceOptionButtonADMIN" :
                                                    isSelected ? "DeviceOptionButtonSelected" : "DeviceOptionButton"}
                                            onClick={() => {
                                                if(newUserType.toUpperCase() !== ADMIN) {
                                                    userDevices[i].isSelected = !userDevices[i].isSelected;
                                                    setUserDevices(userDevices);
                                                    setUpdatedUD(true);
                                                }
                                            }}
                                        ></span>
                                        <span 
                                            className={"DeviceOptionLabel"}
                                            data-value={device.deviceID} 
                                            key={i}>
                                            {device.deviceName}
                                        </span>  
                                    </div>
                                )
                            })
                        }
                    </div>
                </div> :
                isAddingUser ?
                <div className="ManageUsersInfoSubBox">
                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            First Name
                        </div>
                        <textarea
                        className="manageUserTextArea"
                        value={newUserFirstName}
                        id={'FIRST_NAME'}
                        onChange={(e) => {setNewUserFirstName(e.target.value.toString());}}
                        spellCheck={false}
                        cols={1}
                        rows={1} >{newUserFirstName}</textarea>
                    </div>

                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            Last Name
                        </div>
                        <textarea
                        className="manageUserTextArea"
                        value={newUserLastName}
                        id={'LAST_NAME'}
                        onChange={(e) => {setNewUserLastName(e.target.value.toString());}}
                        spellCheck={false}
                        cols={1}
                        rows={1} >{newUserLastName}</textarea>
                    </div>

                    
                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            User Type
                        </div>
                        <div className="EditUserTypeSubBox">
                            <button
                                className={(newUserType.toUpperCase() === ADMIN) ? 
                                    "EditUserTypeButtonSelected" : "EditUserTypeButton"}
                                onClick={() => {
                                    setNewUserType(ADMIN);
                                }}
                                >
                                Admin 
                            </button>
                            
                            <button 
                                className={(newUserType.toUpperCase() === BASIC) ? 
                                    "EditUserTypeButtonSelected" : "EditUserTypeButton"}
                                onClick={() => {
                                    setNewUserType(BASIC);
                                }}
                                >
                                Basic
                            </button>
                        </div>    
                    </div>

                    <div className="UserListMainBox">
                        <div className="userTitleText">
                            Password
                        </div>
                        <input
                            className="manageUserTextArea"
                            value={newUserPassword}
                            id={'NEW_PASSWORD'}
                            type="password"
                            onChange={(e) => {setNewUserPassword(e.target.value.toString());}}
                            >
                        </input>
                    </div>

                    
                    <div className="DeviceOptionMainBox">
                        <div className="DeviceOptionTitleText">
                            <div>Devices</div>
                            <span className={"DeviceOptionButtonLegend"}></span>
                            <span className={"DeviceOptionLegendEqualsText"}>=</span>
                            <span className={"DeviceOptionLegendSelectedText"}>Selected</span>
                        </div>
                        { 
                            devices.map((device, i) => {
                                let isSelected = false;
                                userDevices.forEach((ud, i) => {
                                    if((ud.deviceID === device.deviceID)) {
                                        isSelected = ud.isSelected;
                                    }
                                })
                                
                                return (
                                    <div className={(i === 0) ? "DeviceOptionTopSubBox" : "DeviceOptionSubBox"} key={i}>
                                        <span 
                                            className={
                                                (newUserType.toUpperCase() === ADMIN) ? "DeviceOptionButtonADMIN" :
                                                    isSelected ? "DeviceOptionButtonSelected" : "DeviceOptionButton"}
                                            onClick={() => {
                                                if(newUserType.toUpperCase() !== ADMIN) {
                                                    userDevices[i].isSelected = !userDevices[i].isSelected;
                                                    setUserDevices(userDevices);
                                                    setUpdatedUD(true);
                                                }
                                            }}
                                        ></span>
                                        <span 
                                            className={"DeviceOptionLabel"}
                                            data-value={device.deviceID} 
                                            key={i}>
                                            {device.deviceName}
                                        </span>  
                                    </div>
                                )
                            })
                        }
                    </div>
                </div> :
                <div className="ManageUsersInfoSubBox">
                    {
                        users.map((user, i) => {
                            return (
                                <div className="UserListMainBox" key={i}>
                                    <div className="userTitleText">
                                        {user.userFirstName} {user.userLastName}
                                    </div>
                                    <div className="userTypeText">
                                        {user.userType.toUpperCase()}
                                    </div>
                                    <button 
                                        className="userInfoEditButton"
                                        onClick={(e) => {
                                                selectUser(user);
                                                updateUser(true);
                                            }}
                                            > edit
                                    </button>
                                </div>
                            )
                        })
                    }
                </div>
            }
            {   
                updateUserAttempt ? null :
                addUserAttempt ? null : 
                isUpdatingUser ? 
                
                    isDeletingUser ? 
                    <div className="ManageUsersButtonBox">
                        <div className="ManageUsersDeleteText">
                            Confirm User Deletion
                        </div>
                        <button 
                            className="ManageUsersDeleteButton"
                            onClick={(e) => {
                                setIsDeletingUser(false);
                                setUpdatePassword(false);
                                updateUserInDB(true);
                            }}>
                                Yes, Delete User.
                        </button>
                        <button 
                            className="ManageUsersDeleteButton"
                            onClick={(e) => {
                                setIsDeletingUser(false);
                            }}>
                                No, Cancel.
                        </button>
                    </div>  :
                    <div className="ManageUsersButtonBox">
                        <button 
                            className="ManageUserPrimaryButton"
                            onClick={(e) => {
                                const oldUserPasswordHashed = shajs('sha256').update(oldUserPassword).digest('hex');

                                if((updatePassword && 
                                        (oldUserPasswordHashed === selectedUser.userPassword) && (newUserPassword !== '')) ||
                                    !updatePassword) {
                                    updateUserInDB(false);
                                } else {
                                    setUpdateUserAttempt(true);
                                    setUpdatedCorrectly(false);
                                    setUpdatePassword(false);
                                    setOldUserPassword('');
                                    setNewUserPassword('');
                                    setError('Error Entering New Password.')
                                }
                            }}>
                                Save and Update User
                        </button>
                        <button 
                            className="ManageUsersSecondaryButton"
                            onClick={(e) => {
                                updateUser(false);
                                setUpdatePassword(false);
                                setOldUserPassword('');
                                setNewUserPassword('');
                            }}>
                                Cancel All Changes and Exit
                        </button>
                        <button 
                            className="ManageUsersDeleteButton"
                            onClick={(e) => {
                                setIsDeletingUser(true);
                            }}>
                                Delete User
                        </button>
                    </div> 
                : isAddingUser ? 
                    <div className="ManageUsersButtonBox">
                        <button 
                            className="ManageUserPrimaryButton"
                            onClick={(e) => {
                                if(
                                    newUserFirstName !== '' &&
                                    newUserLastName !== '' &&
                                    newUserPassword !== '' &&
                                    newUserType !== ''
                                ) {
                                    addUserToDB();
                                } else {
                                    setAddUserAttempt(true);
                                    setAddedCorrectly(false);
                                    setNewUserType('');
                                    setError('Some Fields are Empty.')
                                }
                            }}>
                                Save and Add User
                        </button>
                        <button 
                            className="ManageUsersSecondaryButton"
                            onClick={(e) => {
                                addUser(false);
                            }}>
                                Cancel
                        </button>
                    </div> :
                    <div className="ManageUsersButtonBox">
                        <button 
                            className="ManageUserPrimaryButton"
                            onClick={(e) => {
                                addUser(true);
                                setNewUserFirstName('');
                                setNewUserLastName('');
                                setOldUserPassword('');
                                setNewUserPassword('');
                                setNewUserType('');
                                setSelectedUserID(0);
                            }}>
                                Add User
                        </button>
                        <button 
                            className="ManageUsersSecondaryButton"
                            onClick={(e) => {
                                manageUsers(false);
                            }}>
                                Exit
                        </button>
                    </div>
            }
        </div>
    )
}

export default ManageUsers;