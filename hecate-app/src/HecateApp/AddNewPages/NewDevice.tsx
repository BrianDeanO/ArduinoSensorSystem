import React, { useCallback, useEffect, useState } from "react";
import { DeviceType, DeviceDTOType, UserType, UserDeviceType } from "../../interfaces";
import { proxyURL, ADMIN, userTable } from "../../Variables";
import axios from "axios";

interface NewDeviceProps {
    addDevice: (addingDevice: boolean, reset: boolean) => void;
}

const NewDevice: React.FC<NewDeviceProps> = ({ addDevice }: NewDeviceProps) => {  

    const [newDevice, setNewDevice] = useState({} as DeviceDTOType);
    const [newDeviceName, setNewDeviceName] = useState('');
    const [newDeviceType, setNewDeviceType] = useState('');
    const [newDeviceZipCode, setNewDeviceZipCode] = useState('');
    const [devicePostAttempt, setDevicePostAttempt] = useState(false);
    // const [postedCorrectly, setPostedCorrectly] = useState(false);
    const [postError, setPostError] = useState(false);
    const [userDevicesAddedCorrectly, setUserDevicesAddedCorrectly] = useState(false);
    const [users, setUsers] = useState([] as UserType[]);
    const [userSelectionArray, setUserSelectionArray] = useState([] as number[]);

    console.log('TOP OF NEW DEIVEC');

    async function getUsers() {
        let tempUsers: UserType[] = [];
        let tempUserSelections: number[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/User`,
        })
            .then(function (response) {
                console.log('response', response);
                setUsers(response.data);
                tempUsers = response.data;
            }).catch(error => {
                console.log(error);
            })

        if(tempUsers.length > 0) {
            setUserSelectionArray(new Array(tempUsers.length).fill(0));
        }
    }

    async function postUserDevice(userID: number, deviceID: number) {
        console.log('POSTING USER DEVICE')
        await axios.post(
            `${proxyURL}/api/UserDevice?userId=${userID}&deviceId=${deviceID}`, {
                userID: userID,
                deviceID: deviceID
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            console.log('POSTED USER DEVICE', response);
            // setPostedCorrectly(true);
        }).catch(function (error) {
            console.log(error);
            setPostError(error.code);
            setUserDevicesAddedCorrectly(false);
        });
    }

    async function postDevice() {

        let postedCorrectly = false;
        console.log('DEVICE IN POST DEVICE');

        await axios.post(`${proxyURL}/api/Device`, {
            deviceName: newDeviceName,
            deviceType: newDeviceType,
            zipCode: newDeviceZipCode
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            console.log('response', response);
            // setPostedCorrectly(true);
            postedCorrectly = true;
        }).catch(function (error) {
            console.log(error);
            setPostError(error.code)
        });

        // return true or false depending on post result
        // If true, replace the screen with "NEW DEVICE ADDED" with a 
        //      timeout of 3-4 seconds or give option to cancel.
        if(postedCorrectly) {
            let gotDeviceWithName = false;
            console.log('POSTED CORRECTLY');
            // setPostedCorrectly(false);

            let tempUsers: UserType[] = [];
            let newlyAddedDevice: DeviceType;

            // NOW GET THE DEVICE, USING NAME API
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User`,
            })
                .then(function (response) {
                    console.log('GOT USERS', response);
                    // setUsers(response.data);
                    tempUsers = response.data;
                }).catch(error => {
                    console.log(error);
                });


            // NOW GET ALL USERS THAT WERE SELECTED IN THE FORM
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Device/DeviceName?deviceName=${newDeviceName}`,
            })
                .then(function (response) {
                    console.log('GOT DEVICE WITH NAME', response);
                    // setUsers(response.data);
                    newlyAddedDevice = response.data;
                    gotDeviceWithName = true;
                }).catch(error => {
                    console.log(error);
                });

            // THEN FOR EACH USER AND DEVICE ID ADD A USERDEVICE ENTRY  
            /*
                CURRENTLY JUST ASSIGNING DEVICES TO ADMINS
            */
            if(gotDeviceWithName) {
                console.log('tmep users', tempUsers)
                tempUsers.forEach((user, i) => {
                    if(user.userType.toUpperCase() === ADMIN) {
                        postUserDevice(user.userID, newlyAddedDevice.deviceID);
                    }
                });

                setUserDevicesAddedCorrectly(true);
            }

            // WE ARE SUCCESSFUL IN CREATING THE NEW DEVICE.
            //     THEN ADDING THE NEW DEVICE TO ALL ADMIN USERS
            
            // addDevice(false);
        }
        
        setDevicePostAttempt(true);
        setNewDeviceType("");
        setNewDeviceName("");
        setNewDeviceZipCode("");

        console.log('AFTER POST DEVICE');
    }

    // useEffect(() => {
    //     getUsers();
    // })

    return (
        <div className="NewDeviceMainBox">
                {
                    devicePostAttempt ? 
                    <div className="mainDeviceInfoBox"> 
                        <div className="NewDeviceErrorText">
                            {userDevicesAddedCorrectly ? 
                                <div className="NewDeviceErrorSubText">   
                                    <span>
                                        {`Device Added`}
                                    </span>
                                </div> : 
                                <div className="NewDeviceErrorSubText">
                                    <span> { `${postError}.`} </span>
                                    <span> Device Not Added </span>
                                </div>                                
                            }
                        </div>
                        <span 
                            className="NewDeviceExitButton"
                            onClick={(e) => {
                                addDevice(false, true);
                            }}
                        >
                            X
                        </span>
                    </div> : 
                        <div className="mainDeviceInfoBox">
                            <div className="newDeviceTextEntryFieldsBox">
                                <div className="deviceNameEntryBox">
                                    <div className="newDeviceTitleText">
                                        Device Name
                                    </div>

                                    <textarea
                                        className="newDeviceInputTextArea"
                                        value={newDeviceName}
                                        id={'DEVICE_NAME'}
                                        onChange={(e) => {setNewDeviceName(e.target.value.toString());}}
                                        cols={1}
                                        rows={1}></textarea>
                                </div>
                                <div className="deviceTypeEntryBox">
                                    <div className="newDeviceTitleText">
                                        Device Type
                                    </div>
                                    <textarea
                                        className="newDeviceInputTextArea"
                                        value={newDeviceType}
                                        id={'DEVICE_TYPE'}
                                        onChange={(e) => {setNewDeviceType(e.target.value.toString());}}
                                        cols={1}
                                        rows={1}></textarea>
                                </div>
                                <div className="deviceZipCodeEntryBox">
                                    <div className="newDeviceTitleText">
                                        Zip Code
                                    </div>
                                    <textarea
                                        className="newDeviceInputTextArea"
                                        value={newDeviceZipCode}
                                        id={'DEVICE_ZIPCODE'}
                                        onChange={(e) => {setNewDeviceZipCode(e.target.value.toString());}}
                                        cols={1}
                                        rows={1}></textarea>
                                </div>
                            </div>

                            {/* <div className="NewDeviceUserSelectionMainBox">
                                            
                                <legend>Users</legend>
                                {
                                    userTable.map((user, i) => {
                                        return(
                                            // <div className="UserSelectionSubBox">
                                            //     <span 
                                            //         className={(false) ? "UserSelectionCircle" : "UserSelectionCircleActive"}
                                            //         onClick={(e) => {
                                                        
                                            //         }}
                                            //     ></span>
                                            //     <div className="UserSelectionText">
                                            //         User ID - {user.user_ID}
                                            //     </div>
                                            // </div>
                                            <fieldset>
                                                <input type="checkbox" id="chbx" name="agree" value={user.user_ID} />
                                                <label for="chbx">User - {user.user_ID}</label>
                                            </fieldset>
                                        )
                                    })
                                }
                            </div> */}
                            {/* 
                            
                                ADD A USER SELECTION BOX TO MAKE USERDEVICE ENTRIES.
                                CIRCLE METHOD MIFHT BE HARD.
                                TRY SELECTS WITH OPTIONS
                                AND FOR EACH NEW ONE CHOSEN, HAVE A BUTTON FOR, ANTOEHR USER??
                            */}
                            {/* 
                            
                                Add a sensor creator option? To make new sensors
                                to attach to the device, right then and there. 
                                    OR
                                Just leave it as is, where we make them separately. <----this one is MUCH easier
                            
                            */}
                        </div>
            }
            {
                devicePostAttempt ? null :
                <div className="DeviceSubmitAndCancelMainBox">
                    <button 
                        className="mainButton"
                        onClick={(e) => {
                            console.log("newDeviceName",newDeviceName)
                            console.log("newDeviceType", newDeviceType)
                            console.log("newDeviceZipCode", newDeviceZipCode)
                            postDevice();
                        }}>
                            Submit
                    </button>
                    <button 
                        className="mainButton"
                        onClick={(e) => {
                            addDevice(false, true)
                        }}>
                            Cancel
                    </button>
                </div>
            }

        </div>
    )
}

export default NewDevice;