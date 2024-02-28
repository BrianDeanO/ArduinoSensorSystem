import React, { useEffect, useState, useCallback } from "react";
import { proxyURL, ADMIN, BASIC } from "../../variables.js";
import axios from "axios";
import { UserType, UserDeviceSelectedType, DeviceType, UserDeviceType } from "../../interfaces.js";
import UserDevices from "./UserDevices.tsx";

interface ManageUsersProps {
    manageUsers: (isManagingUsers: boolean) => void;
    isManagingUsers: boolean;
    addUser: (addingUser: boolean) => void;
    isAddingUser: boolean;
    updateUser: (updatingUser: boolean) => void;
    isUpdatingUser: boolean;
}

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
    // const [newUser, setNewUser] = useState({} as UserType);
    const [users, setUsers] = useState([] as UserType[]);

    const [newUserFirstName, setNewUserFirstName] = useState('');
    const [newUserLastName, setNewUserLastName] = useState('');
    const [newUserType, setNewUserType] = useState('');
    const [newUserPassword, setNewUserPassword] = useState('');

    const [updatePassword, setUpdatePassword] = useState(false);
    const [oldUserPassword, setOldUserPassword] = useState('');

    const [updateUserAttempt, setUpdateUserAttempt] = useState(false);
    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);
    const [addUserAttempt, setAddUserAttempt] = useState(false);
    const [addedCorrectly, setAddedCorrectly] = useState(false);
    const [error, setError] = useState('');

    const [userDevices, setUserDevices] = useState([] as UserDeviceSelectedType[]);
    const [devices, setDevices] = useState([] as DeviceType[]);

    const [updatedUD, setUpdatedUD] = useState(false);

    console.log('MANAGE USER RE-RENDER', userDevices)

    useEffect(() => {
        console.log('USING CALLBACK????', userDevices)
        setUserDevices(userDevices);
        setUpdatedUD(false);
    }, [setUserDevices, userDevices, devices, updatedUD])

    const getDevices = useCallback(async() => {

        let tempDevices: DeviceType[] = [];

        if(selectedUserID !== undefined && selectedUserID !== 0) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User/${selectedUserID}/Devices`,
            })
                .then(function (response) {
                    // console.log('response', response);
                    // setDevices(response.data);
                    tempDevices = response.data;
                    console.log('DEVICES FROM AXIOS', tempDevices)
                }).catch(error => {
                    console.log(error);
                })
    
            setDevices(tempDevices);
        }

    }, [selectedUserID])

    const getUserDevices = useCallback(async() => {
        let tempAllUserDevices: UserDeviceType[] = [];
        let tempSelectedUserDevices: UserDeviceSelectedType[] = [];
        let tempDevices: DeviceType[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/UserDevice`,
        })
            .then(function (response) {
                console.log('ALL USER DEVICES', response);
                // setDevices(response.data);
                tempAllUserDevices = response.data;


                console.log('ALL UDS', tempAllUserDevices)

                tempAllUserDevices.forEach((ud, i) => {
                    console.log('ud', ud, selectedUserID);
                    if(selectedUserID === ud.userID) {
                        console.log('pushed ID -', selectedUserID);
                        tempSelectedUserDevices.push({
                            deviceID: ud.deviceID,
                            isSelected: true, 
                        } as UserDeviceSelectedType)
                    }
                })
                console.log('SELECETD UDS', tempSelectedUserDevices)
                setUserDevices(tempSelectedUserDevices);

                // console.log('DEVICES FROM AXIOS', tempUserDevices)
            }).catch(error => {
                console.log(error);
            })

            // console.log('ALL UDS', tempAllUserDevices)
            // setUserDevices(tempAllUserDevices);

            // tempAllUserDevices.forEach((ud, i) => {
            //     if(selectedUserID === ud.userID) {
            //         tempSelectedUserDevices.push({
            //             deviceID: ud.deviceID,
            //             isSelected: true, 
            //         } as UserDeviceSelectedType)
            //     }
            // })
            // console.log('SELECETD UDS', tempSelectedUserDevices)
            

            // if(tempUserDevices.length !== 0) {
            //     await axios({
            //         method: 'get',
            //         url: `${proxyURL}/api/User/${selectedUserID}/Devices`,
            //     })
            //         .then(function (response) {
            //             // console.log('response', response);
            //             // setDevices(response.data);
            //             tempDevices = response.data;
            //             // console.log('DEVICES FROM AXIOS', tempDevices)
            //         }).catch(error => {
            //             console.log(error);
            //         })
        
            //     // setDevices(tempDevices);
            // }

            console.log('DEVICES', tempDevices)

    }, [selectedUserID])


    useEffect(() => {
        getUserDevices();
        getDevices();
        // setDevices(deviceTable);
    }, [selectedUserID, getUserDevices, getDevices])

    useEffect(() => {
        console.log('USING CALLBACK????', userDevices)
        setUserDevices(userDevices);
    }, [setUserDevices, userDevices, devices])



    function selectUser(user: UserType) {
        console.log('user', user);
        setSelectedUser(user);
        setSelectedUserID(user.userID);
        setNewUserFirstName(user.userFirstName);
        setNewUserLastName(user.userLastName);
        setNewUserType(user.userType);
        console.log('newUser Type set', newUserType)
    }

    const getUsers = useCallback(async() => {
        if(isManagingUsers) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User`,
            })
            .then(function (response) {
                console.log('response', response);
                setUsers(response.data);
            }).catch(error => {
                console.log(error);
            })
        }
        
    }, [isManagingUsers]);

    async function updateUserInDB() {

        console.log('UPDATE USER IN FUNCTION')
        setUpdateUserAttempt(true);

        await axios.put(`${proxyURL}/api/User/${selectedUserID}`, {
            userID: selectedUserID,
            userType: newUserType,
            userFirstName: newUserFirstName,
            userLastName: newUserLastName,
            userPassword: updatePassword ? newUserPassword : selectedUser.userPassword,
            userEmail: '',
            userPhone: '',
            userNotification: false,
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            console.log('response', response);
            setUsers(response.data);
            setUpdatedCorrectly(true);
        }).catch(error => {
            console.log(error);
            setError(error);
        })

        updateUser(false);
        resetState();
        getUsers();
    }

    async function addUserToDB() {
        console.log('ADD USER IN FUNCTION')
        setAddUserAttempt(true);
        
        await axios.post(`${proxyURL}/api/User`, {
            userType: newUserType,
            userFirstName: newUserFirstName,
            userLastName: newUserLastName,
            userPassword: newUserPassword,
            userEmail: '',
            userPhone: '',
            userNotification: false,
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            console.log('response', response);
            setUsers(response.data);
            setAddedCorrectly(true);
        }).catch(error => {
            console.log(error);
            setError(error)
        })

        addUser(false);
        resetState();
        getUsers();
    }

    function resetState() {
        // setSelectedUser({} as UserType);
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
    }, [getUsers])

    return(
        <div className="ManageUsersMainBox">
            {
                updateUserAttempt ?
                    <div className="UserActionResultMainBox"> 
                        <div className="UserActionResultErrorText">
                            {updatedCorrectly ?  
                                <div className="UserActionResultErrorSubText">   
                                    <span >
                                        Successfully Updated User.
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
                                <div className="UpdatingPasswordSubBox">
                                    <div className="userTitleText">
                                        Old Password
                                    </div>
                                    <input
                                        className="manageUserTextArea"
                                        value={oldUserPassword}
                                        id={'OLD_PASSWORD'}
                                        type="password"
                                        onChange={(e) => {setOldUserPassword(e.target.value.toString());}}
                                        >
                                    </input>
                                </div>

                                <div className="UpdatingPasswordSubBox">
                                    <div className="userTitleText">
                                        New Password
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
                            </div> 
                        : null
                    }
                    {/* <UserDevices 
                        devices={devices}
                        userDevices={userDevices}
                        setUserDevices={setUserDevices}
                    /> */}
                    <div className="DeviceOptionMainBox">
                        <div className="userTitleText">
                            Devices
                        </div>
                        { 
                            devices.map((device, i) => {
                                // console.log('UD ', i, userDevices[i].isSelected)
                                let isSelected = false;
                                userDevices.forEach((ud, i) => {
                                    if((ud.deviceID === device.deviceID)) {
                                        isSelected = ud.isSelected;
                                    }
                                })

                                console.log('isSelected', isSelected)
                                return (
                                    <div className="DeviceOptionSubBox" key={i}>
                                        <span 
                                            className={isSelected ? "DeviceOptionButtonSelected" : "DeviceOptionButton"}
                                            onClick={() => {
                                                console.log('i', i)
                                                console.log('userDevices i', userDevices[i])
                                                console.log('userDevices[i].isSelected', userDevices[i].isSelected);
                                                console.log('userDevices', userDevices)
                                                
                                                userDevices[i].isSelected = !userDevices[i].isSelected;
                                                console.log('userDevices[i].isSelected', userDevices[i].isSelected);
                                                console.log('userDevices i', userDevices[i])
                                                console.log('userDevices', userDevices)
                                                setUserDevices(userDevices);
                                                // setDevices(devices);
                                                setUpdatedUD(true);
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
                                        {user.userType}
                                    </div>
                                    <button 
                                        className="userInfoEditButton"
                                        onClick={(e) => {
                                                selectUser(user);
                                                updateUser(true);
                                                // getUserDevices();
                                                console.log('SELECTED UESR', user)
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
                    <div className="ManageUsersButtonBox">
                        <button 
                            className="ManageUserPrimaryButton"
                            onClick={(e) => {
                                console.log('UPDATING USER')
                                console.log('old - ', oldUserPassword);
                                console.log('new - ', newUserPassword);
                                if((updatePassword && 
                                        (oldUserPassword === selectedUser.userPassword) && (newUserPassword !== '')) ||
                                    !updatePassword) {
                                        console.log("PASSWORD IS GOOD");
                                    updateUserInDB();
                                } else {
                                    setUpdateUserAttempt(true);
                                    setUpdatedCorrectly(false);
                                    setUpdatePassword(false);
                                    setOldUserPassword('');
                                    setNewUserPassword('');
                                    setError('Error Entering New Password.')
                                }
                            }}>
                                Save and Update
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
                    </div> 
                : isAddingUser ? 
                    <div className="ManageUsersButtonBox">
                        <button 
                            className="ManageUserPrimaryButton"
                            onClick={(e) => {
                                addUserToDB();
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
                    </div> 
                :
                    <div className="ManageUsersButtonBox">
                        <button 
                            className="ManageUserPrimaryButton"
                            onClick={(e) => {
                                addUser(true);
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