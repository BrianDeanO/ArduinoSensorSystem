import React, { useCallback, useEffect, useState, } from "react";
import { DeviceType } from "../interfaces";
import { proxyURL } from "../variables.js";
import axios from "axios";

interface DeviceProps {
    loggedInUserID: number;
    selectDevice: ((selectedDeviceID: number, resetSensorID: boolean) => void);
    selectedDeviceID: number;
    configureDevice: ((configuringDevice: boolean, resetDevices: boolean) => void);
    isAdmin: boolean;
    configuringSensor: boolean;
    isManagingUsers: boolean;
    isLoggingOut: boolean;
}

const Devices: React.FC<DeviceProps> = ({
    loggedInUserID, 
    selectDevice,
    selectedDeviceID,
    configureDevice,
    isAdmin,
    configuringSensor,
    isManagingUsers,
    isLoggingOut
}: DeviceProps) => {  
    const [devices, setDevices] = useState([] as DeviceType[]);

    const getDevices = useCallback(async(loggedInUserID: number) => {
        let tempAllDevices: DeviceType[] = [];
        let tempNonDeletedDevices: DeviceType[] = [];

        if(loggedInUserID !== undefined && loggedInUserID !== 0) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User/${loggedInUserID}/Devices`,
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

            if(isManagingUsers || isLoggingOut) {
                selectDevice(0, true);
            } else {
                selectDevice(selectedDeviceID || 0, false);
            }
        }
    }, [selectDevice, selectedDeviceID, isManagingUsers, isLoggingOut])

    useEffect(() => {
        getDevices(loggedInUserID);
    }, [loggedInUserID, getDevices])

    return (
        <div className="MainDevicesBox">
            <div className="MainDeviceSelectorBox">
                <div className="deviceSelectorTitleText">
                    Devices
                </div>
                {
                    ((loggedInUserID === 0) || loggedInUserID === undefined) ? null :
                        devices.map((device, i) => {
                            return (
                                <div className="DeviceSelectorSubBox" key={i}>

                                    <span 
                                        className={((device.deviceID === selectedDeviceID)) ? "SelectedDeviceButton" : "DeviceButton"}
                                        data-value={device.deviceID} 
                                        key={i}
                                        onClick={(e) => {
                                            const tempStringID = (e.target as HTMLElement).getAttribute("data-value");
                                            const tempDeviceID = parseInt((tempStringID != null) ? tempStringID : "");
                                            if(!configuringSensor && !isManagingUsers) {
                                                if(selectedDeviceID === tempDeviceID) {
                                                    selectDevice(0, true);
                                                } else {
                                                    selectDevice(tempDeviceID, true);
                                                }
    
                                                configureDevice(false, false);
                                            }
                                    }}>
                                        {device.deviceName}
                                    </span>   

                                    {
                                        isAdmin ? 
                                            ((selectedDeviceID === device.deviceID ? 
                                                <span 
                                                    className="ConfigureDeviceButton"
                                                    data-value={selectedDeviceID}
                                                    onClick={(e) => {
                                                        if(!configuringSensor && !isManagingUsers) {
                                                            configureDevice(true, false);
                                                        }
                                                }}>
                                                    Configure Device
                                                </span> : null
                                        )) : null
                                    }
                                </div>
                            )
                        })
                }
            </div>
        </div>  
    )
}

export default Devices;
