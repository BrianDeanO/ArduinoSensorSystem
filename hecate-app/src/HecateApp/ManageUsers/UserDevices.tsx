
import React, { useEffect, useState, useCallback } from "react";
import { proxyURL, ADMIN, BASIC } from "../../variables.js";
import axios from "axios";
import { UserType, UserDeviceSelectedType, DeviceType, UserDeviceType } from "../../interfaces.js";

interface UserDevicesProps {
    devices: DeviceType[]
    userDevices: UserDeviceSelectedType[]
    setUserDevices: (userDevices: UserDeviceSelectedType[]) => void;
}

const UserDevices: React.FC<UserDevicesProps>  = ({
    devices,
    userDevices,
    setUserDevices
}: UserDevicesProps) => {

    // console.log('RE RENDINGER THE DEVICES THING', devices)

    // const [userDevices, setUserDevices] = useState([] as UserDeviceSelectedType[]);
    // const [devices, setDevices] = useState([] as DeviceType[]);
    const [updatedUD, setUpdatedUD] = useState(false);
    
    useEffect(() => {
        console.log('USING CALLBACK????', userDevices)
        setUserDevices(userDevices);
        setUpdatedUD(false);
    }, [setUserDevices, userDevices, devices, updatedUD])


    return (
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
                                // setDevices(devices)
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
    )
}

export default UserDevices;