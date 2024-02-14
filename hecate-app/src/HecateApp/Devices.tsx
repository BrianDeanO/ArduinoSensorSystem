import React, { useCallback, useEffect, useState, } from "react";
import { DeviceType } from "../interfaces";
import { proxyURL, deviceTable } from "../Variables";
import axios from "axios";


interface DeviceProps {
    loggedInUserID: number;
    selectDevice: (selectedDeviceID: number, resetSensorID: boolean) => void;
    selectedDeviceID: number;
    addDevice: (addingDevice: boolean, reset: boolean) => void;
    configureDevice: (configuringDevice: boolean) => void;
}

const Devices: React.FC<DeviceProps> = ({
    loggedInUserID, 
    selectDevice,
    selectedDeviceID,
    addDevice,
    configureDevice
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
                // setDevices(response.data);
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
        // setDevices(deviceTable);
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

                                        if(selectedDeviceID === tempDeviceID) {
                                            selectDevice(0, true);
                                        } else {
                                            selectDevice(tempDeviceID, true);
                                        }

                                        configureDevice(false);
                                        // addDevice(false, false);
                                }}>
                                    {device.deviceName}
                                </span>   

                                {
                                    (selectedDeviceID === device.deviceID ? 
                                        <span 
                                            className={"ConfigureDeviceButton"}
                                            data-value={selectedDeviceID}
                                            onClick={(e) => {
                                                const tempStringID = (e.target as HTMLElement).getAttribute("data-value");
                                                const tempDeviceID = parseInt((tempStringID != null) ? tempStringID : "");
                                                configureDevice(true);
                                                // if(selectedDeviceID === tempDeviceID) {  
                                                //     configureDevice(0, true);
                                                // } else {
                                                //     configureDevice(tempDeviceID, true);
                                                // }
                                        }}>
                                            Configure Device
                                        </span>  
                                        
                                        : null
                                    )
                                }
                            </div>
                        )
                    })
                }
            </div>
            {/* <div className="AddDeviceBox">
                <span 
                    className={"DeviceButton"}
                    onClick={(e) => {
                        addDevice(true, true);
                }}>
                    Add New Device
                </span>   
            </div> */}
        </div>  
    )
}

export default Devices;
