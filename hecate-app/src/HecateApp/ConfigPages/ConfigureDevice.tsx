import React, { useCallback, useEffect, useState } from "react";
import { SensorType, SensorDTOType, DeviceType } from "../../interfaces";
import { channelCountArray, proxyURL } from "../../Variables";
import axios from "axios";

interface ConfigureDeviceProps {
    configureDevice: (configuringDevice: boolean) => void;
    selectedDeviceID: number;
}

const ConfigureDevice: React.FC<ConfigureDeviceProps> = (
    {
    configureDevice,
    selectedDeviceID
}: ConfigureDeviceProps
) => {  

    const [newDeviceName, setNewDeviceName] = useState('');
    const [newDeviceType, setNewDeviceType] = useState('');
    const [newDeviceZipCode, setNewDeviceZipCode] = useState('');
    const [deviceUpdateAttempt, setDeviceUpdateAttempt] = useState(false);
    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);
    const [postError, setPostError] = useState(false);
    const [userDevicesAddedCorrectly, setUserDevicesAddedCorrectly] = useState(false);
    const [userSelectionArray, setUserSelectionArray] = useState([] as number[]);

    async function updateDevice() {

        // let updatedCorrectly = false;
        console.log('DEVICE IN POST DEVICE');

        await axios.put(`${proxyURL}/api/Device${selectedDeviceID}`, {
            deviceID: selectedDeviceID,
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
            setUpdatedCorrectly(true);
            // updatedCorrectly = true;
        }).catch(function (error) {
            console.log(error);
            setPostError(error.code)
        });

        // if(updatedCorrectly) {
 

        //     // WE ARE SUCCESSFUL IN CREATING THE NEW DEVICE.
        //     //     THEN ADDING THE NEW DEVICE TO ALL ADMIN USERS
            
        //     // addDevice(false);
        // }
        
        setDeviceUpdateAttempt(true);
        setNewDeviceType("");
        setNewDeviceName("");
        setNewDeviceZipCode("");

        console.log('AFTER POST DEVICE');
    }

    return (
        <div className="ConfigDeviceMainBox">
            configuring device ID - {selectedDeviceID}
        {  
            deviceUpdateAttempt ? null :  
            <div className="DeviceSubmitAndCancelMainBox">
                <button 
                    className="mainButton"
                    onClick={(e) => {
                        updateDevice();
                    }}>
                        Update Device
                </button>
                <button 
                    className="mainButton"
                    onClick={(e) => {
                        configureDevice(false)
                    }}>
                        Cancel
                </button>
            </div>
        }
        </div>
    )
}

export default ConfigureDevice;