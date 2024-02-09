import React, { useEffect, useState, } from "react";
import { DeviceType } from "../interfaces";

interface DeviceProps {
    loggedInUserID: number;
    selectDevice: (selectedDeviceID: number) => void;
    selectedDeviceID: number;
}

const Devices: React.FC<DeviceProps> = ({
    loggedInUserID, 
    selectDevice,
    selectedDeviceID
}: DeviceProps) => {  

    const [devices, setDevices] = useState([] as DeviceType[]);
    // const [selectedDeviceID, setSelectedDeviceID] = useState();

    console.log("loggedIn IN DEVICES", loggedInUserID);  
    let device = 'not clciekd';

    useEffect(() => {
        getDevices(loggedInUserID);
    }, [loggedInUserID])


    console.log('SELECTED DEVICE ', selectedDeviceID);

    const getDevices = (loggedInUserID: number) => {

        /**********
          USE AXIOS TO GET DEVICES

        *************/
        const deviceArray = [
            {
                deviceID: 1,
                deviceName: 'DEVICE_1'
            },
            {
                deviceID: 2,
                deviceName: 'DEVICE_2'
            }
        ] as DeviceType[];

        setDevices(deviceArray);
        selectDevice(0);
    }

    return (
        <div className="MainDevicesBox">
            <div className="MainDeviceSelectorBox">
                DEVICES
                <div>
                    {`\nUSER ID - ${loggedInUserID || 0}`}
                </div>
                
                {
                    ((loggedInUserID === 0) || loggedInUserID === undefined) ? null :
                    devices.map((device, i) => {
                        return (
                            <span 
                                className={((device.deviceID === selectedDeviceID)) ? "SelectedDeviceNameBox" : "DeviceNameBox"}
                                value={device.deviceID} 
                                key={i} 
                                onClick={(e) => {
                                    const tempStringID = (e.target as HTMLElement).getAttribute("value");
                                    const tempDeviceID = parseInt((tempStringID != null) ? tempStringID : "");

                                    selectDevice(tempDeviceID);
                            }}>
                                {device.deviceName}
                            </span>   
                        )
                    })
                }
            </div>
        </div>  
    )
}

export default Devices;