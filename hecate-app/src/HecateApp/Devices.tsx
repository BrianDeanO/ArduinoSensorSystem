import React, { useCallback, useEffect, useState, } from "react";
import { DeviceType } from "../interfaces";
import { proxyURL } from "../Variables";
import axios from "axios";

interface DeviceProps {
    loggedInUserID: number;
    selectDevice: (selectedDeviceID: number, resetSensorID: boolean) => void;
    selectedDeviceID: number;
    addDevice: (addingDevice: boolean) => void;
}

const Devices: React.FC<DeviceProps> = ({
    loggedInUserID, 
    selectDevice,
    selectedDeviceID,
    addDevice
}: DeviceProps) => {  
    const [devices, setDevices] = useState([] as DeviceType[]);

    const getDevices = useCallback(async(loggedInUserID: number) => {

        let tempDevices: DeviceType[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/User/${loggedInUserID}/Devices`,
        })
            .then(function (response) {
                // console.log('response', response);
                setDevices(response.data);
                tempDevices = response.data;
                // console.log('DEVICES FROM AXIOS', tempDevices)
            }).catch(error => {
                console.log(error);
            })

        setDevices(tempDevices);
        // console.log('SET DEVICES', tempDevices)
        selectDevice(selectedDeviceID || 0, false);

    }, [selectDevice, selectedDeviceID])

    useEffect(() => {
        getDevices(loggedInUserID);
    }, [loggedInUserID, getDevices])

    return (
        <div className="MainDevicesBox">
            <div className="MainDeviceSelectorBox">

            DEVICES
                <div className="deviceSelectorTitleText">
                    {`\nUSER ID - ${loggedInUserID || 0}`}
                </div>
                
                {
                    ((loggedInUserID === 0) || loggedInUserID === undefined) ? null :
                    devices.map((device, i) => {
                        return (
                            <span 
                                className={((device.deviceID === selectedDeviceID)) ? "SelectedDeviceButton" : "DeviceButton"}
                                data-value={device.deviceID} 
                                key={i} 
                                onClick={(e) => {
                                    const tempStringID = (e.target as HTMLElement).getAttribute("data-value");
                                    const tempDeviceID = parseInt((tempStringID != null) ? tempStringID : "");

                                    if(selectedDeviceID === tempDeviceID) {
                                        selectDevice(0, true);
                                    } else {
                                        selectDevice(tempDeviceID, true);
                                    }
                            }}>
                                {device.deviceName}
                            </span>   
                        )
                    })
                }
            </div>
            <div className="AddDeviceBox">
                <span 
                    className={"DeviceButton"}
                    onClick={(e) => {
                        addDevice(true);
                }}>
                    Add New Device
                </span>   
            </div>
        </div>  
    )
}

export default Devices;
