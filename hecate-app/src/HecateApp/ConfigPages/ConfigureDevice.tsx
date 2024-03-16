import React, { useCallback, useEffect, useState } from "react";
import { DeviceType } from "../../interfaces";
import { proxyURL, timeFrameConstants } from "../../variables.js";
import axios from "axios";

interface ConfigureDeviceProps {
    configureDevice: ((configuringDevice: boolean, resetDevices: boolean) => void);
    selectedDeviceID: number;
}

const ConfigureDevice: React.FC<ConfigureDeviceProps> = (
    {
    configureDevice,
    selectedDeviceID
}: ConfigureDeviceProps
) => {  

    const [newDeviceName, setNewDeviceName] = useState('');
    const [isDeviceNameEdit, setIsDeviceNameEdit ] = useState(false);

    const [newDeviceIdent, setNewDeviceIdent] = useState('');

    const [newDeviceType, setNewDeviceType] = useState('');
    const [isDeviceTypeEdit, setIsNewDeviceTypeEdit] = useState(false);

    const [newDeviceZipCode, setNewDeviceZipCode] = useState('');
    const [isDeviceZipCodeEdit, setIsDeviceZipCodeEdit] = useState(false);

    const [newDeviceUpdateIntervalValue, setNewDeviceUpdateIntervalValue] = useState(1);
    const [newDeviceUpdateIntervalLabel, setNewDeviceUpdateIntervalLabel] = useState('');
    const [newDeviceUpdateInterval, setNewDeviceUpdateInterval] = useState(1);
    const [isUpdateIntervalEdit, setIsUpdateIntervalEdit] = useState(false);

    const [isDeletingDevice, setIsDeletingDevice] = useState(false);

    const [deviceLastSeen, setDeviceLastSeen] = useState('');
    const [deviceLastSeenSeconds, setDeviceLastSeenSeconds] = useState('');
    const [deviceLastSeenMinutes, setDeviceLastSeenMinutes] = useState('');
    const [deviceLastSeenHours, setDeviceLastSeenHours] = useState('');
    const [deviceLastSeenDateString, setDeviceLastSeenDateString] = useState('');

    const [deviceUpdateAttempt, setDeviceUpdateAttempt] = useState(false);

    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);    
    const [updateDeleteMessage, setUpdateDeleteMessage] = useState('');
    const [postError, setPostError] = useState(false);

    const getDevice = useCallback(async(deviceID: number) => {
        let tempDevice: DeviceType;
        await axios({
            method: 'get',
            url: `${proxyURL}/api/Device/${deviceID}`,
        })
            .then(function (response) {
                tempDevice = response.data;
                setNewDeviceName(tempDevice.deviceName);
                setNewDeviceIdent(tempDevice.deviceIdent);
                setNewDeviceType(tempDevice.deviceType);
                setNewDeviceZipCode(tempDevice.deviceZipCode);
                setDeviceLastSeen(tempDevice.deviceLastSeen);

                const tempFullDate = new Date(tempDevice.deviceLastSeen);
                let tempSeconds: string = tempFullDate.getSeconds().toString();
                let tempMinutes: string = tempFullDate.getMinutes().toString();
                let tempHours: string = tempFullDate.getHours().toString();
                let tempDateString: string = tempFullDate.toDateString();

                if(tempSeconds.length === 1) {
                    tempSeconds = `0${tempSeconds}`
                }
                if(tempMinutes.length === 1) {
                    tempMinutes = `0${tempMinutes}`
                }
                if(tempHours.length === 1) {
                    tempHours = `0${tempHours}`
                }

                setDeviceLastSeenSeconds(tempSeconds);
                setDeviceLastSeenMinutes(tempMinutes);
                setDeviceLastSeenHours(tempHours);
                setDeviceLastSeenDateString(tempDateString);

                parseUpdateInterval(tempDevice.deviceUpdateInterval);
            }).catch(error => {
                console.log(error);
            })
    }, []);


    async function parseUpdateInterval(deviceUpdateInterval: number) {
        let updateValue: number = 1;
        let updateLabel: string = 'DAY';

        if((deviceUpdateInterval % timeFrameConstants.MONTH) === 0) {
            updateValue = deviceUpdateInterval / timeFrameConstants.MONTH;
            updateLabel = 'MONTH';
        }

        else if((deviceUpdateInterval % timeFrameConstants.WEEK) === 0) {
            updateValue = deviceUpdateInterval / timeFrameConstants.WEEK;
            updateLabel = 'WEEK';
        }

        else if((deviceUpdateInterval % timeFrameConstants.DAY) === 0) {
            updateValue = deviceUpdateInterval / timeFrameConstants.DAY;
            updateLabel = 'DAY';
        }

        else if((deviceUpdateInterval % (1000 * 60 * 60)) === 0) {
            updateValue = deviceUpdateInterval / (1000 * 60 * 60);
            updateLabel = 'HOUR';
        }

        else if((deviceUpdateInterval % (1000 * 60)) === 0) {
            updateValue = deviceUpdateInterval / (1000 * 60);
            updateLabel = 'MIN';
        }

        else if((deviceUpdateInterval % 1000) === 0) {
            updateValue = deviceUpdateInterval / 1000;
            updateLabel = 'SEC';
        }

        setNewDeviceUpdateIntervalValue(updateValue);
        setNewDeviceUpdateIntervalLabel(updateLabel);
    }

    async function convertUpdateTime(intervalValue: number, intervalLabel: string) {
        switch(intervalLabel) {
            case 'MONTH': 
                setNewDeviceUpdateInterval(intervalValue * timeFrameConstants.MONTH);
                break;

            case 'WEEK': 
                setNewDeviceUpdateInterval(intervalValue * timeFrameConstants.WEEK);
                break;

            case 'DAY': 
                setNewDeviceUpdateInterval(intervalValue * timeFrameConstants.DAY);
                break;

            case 'HOUR': 
                setNewDeviceUpdateInterval(intervalValue * (60 * 60));
                break;

            case 'MIN':
                setNewDeviceUpdateInterval(intervalValue * 60);
                break;

            case 'SEC': 
                setNewDeviceUpdateInterval(intervalValue);
                break;

            default:
                break;
        }
    }

    useEffect(() => {
        getDevice(selectedDeviceID);
    }, [selectedDeviceID, getDevice])


    async function updateDevice(deletedDevice: boolean) {
        setDeviceUpdateAttempt(true);
        
        await axios.put(`${proxyURL}/api/Device/${selectedDeviceID}`, {
            deviceID: selectedDeviceID,
            deviceIdent: newDeviceIdent,
            deviceName: newDeviceName,
            deviceType: newDeviceType,
            deviceZipCode: newDeviceZipCode,
            deviceUpdateInterval: newDeviceUpdateInterval,
            deviceLastSeen: deviceLastSeen,
            deviceIsDeleted: deletedDevice
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            setUpdatedCorrectly(true);
            setUpdateDeleteMessage(deletedDevice 
                    ? 'Successfully Deleted Device.' : 'Successfully Updated Device.');
        }).catch(function (error) {
            console.log(error);
            setPostError(error.code)
        });

        setNewDeviceName("");
        setNewDeviceIdent("");
        setNewDeviceType("");
        setNewDeviceZipCode("");
        setNewDeviceUpdateIntervalValue(1);
        setNewDeviceUpdateIntervalLabel('');
        setDeviceLastSeen('');
    }


    return (
        <div className="ConfigDeviceMainBox">
            {  
                deviceUpdateAttempt ? 
                <div className="DeviceUpdatedMainBox"> 
                    <div className="UpdateDeviceErrorText">
                        {updatedCorrectly ?  
                            <div className="UpdateDeviceErrorSubText">   
                                <span >
                                    {updateDeleteMessage}
                                </span>
                            </div> : 
                            <div className="UpdateDeviceErrorSubText">
                                <span> { `${postError}`} </span>
                                <span> Unable to Updated Device </span>
                            </div>                                
                        }
                    </div>
                    <span 
                        className="UpdatedDeviceExitButton"
                        onClick={(e) => {
                            configureDevice(false, true);
                        }}
                    >
                        X
                    </span>
                </div>
                :  
                <div className="ConfigDeviceSubBox">
                <div className="DeviceAttributeMainBox">
                    <div className="configDeviceTitleText">
                        Device Last Seen
                    </div>
                    <div className="configDeviceStandardTextBox">
                        <div className="configDeviceStandardText">
                            {`${deviceLastSeenDateString} @ ${deviceLastSeenHours}:${deviceLastSeenMinutes}:${deviceLastSeenSeconds}`}
                        </div>
                    </div>
                </div>
                    <div className="DeviceAttributeMainBox">
                        <div className="configDeviceTitleText">
                            Device Identifier
                        </div>
                        <div className="configDeviceStandardTextBox">
                            <div className="configDeviceStandardText">
                                {newDeviceIdent}
                            </div>
                        </div>
                    </div>

                    <div className="DeviceAttributeMainBox">
                        <div className="configDeviceTitleText">
                            Device Name
                        </div>
                        {isDeviceNameEdit ? 
                            <textarea
                                className="deviceTextArea"
                                value={newDeviceName}
                                id={'NAME'}
                                onChange={(e) => {setNewDeviceName(e.target.value.toString());}}
                                spellCheck={false}
                                cols={1}
                                rows={1}></textarea> : 
                            <div className="configDeviceStandardTextBox">
                                <div className="configDeviceStandardText">
                                    {newDeviceName}
                                </div>
                            </div>
                        }
                        <button 
                            className="deviceInfoEditButton"
                            onClick={(e) => {
                                    setIsDeviceNameEdit(!isDeviceNameEdit);
                                }}
                                >{isDeviceNameEdit ? 'save' : 'edit'}
                        </button>
                    </div>

                    <div className="DeviceAttributeMainBox">
                        <div className="configDeviceTitleText">
                            Device Type
                        </div>
                        {isDeviceTypeEdit ? 
                            <textarea
                                className="deviceTextArea"
                                value={newDeviceType}
                                id={'TYPE'}
                                onChange={(e) => {setNewDeviceType(e.target.value.toString());}}
                                spellCheck={false}
                                cols={1}
                                rows={1}></textarea> : 
                            <div className="configDeviceStandardTextBox">
                                <div className="configDeviceStandardText">
                                    {newDeviceType}
                                </div>
                            </div>
                        }
                        <button 
                            className="deviceInfoEditButton"
                            onClick={(e) => {
                                    setIsNewDeviceTypeEdit(!isDeviceTypeEdit);
                                }}
                                >{isDeviceTypeEdit ? 'save' : 'edit'}
                        </button>
                    </div>

                    <div className="DeviceAttributeMainBox">
                        <div className="configDeviceTitleText">
                            Device Zip Code
                        </div>
                        {isDeviceZipCodeEdit ? 
                            <textarea
                                className="deviceTextArea"
                                value={newDeviceZipCode}
                                id={'ZIPCODE'}
                                onChange={(e) => {setNewDeviceZipCode(e.target.value.toString());}}
                                spellCheck={false}
                                cols={1}
                                rows={1}></textarea> : 
                            <div className="configDeviceStandardTextBox">
                                <div className="configDeviceStandardText">
                                    {newDeviceZipCode}
                                </div>
                            </div>
                        }
                        <button 
                            className="deviceInfoEditButton"
                            onClick={(e) => {
                                    setIsDeviceZipCodeEdit(!isDeviceZipCodeEdit);
                                }}
                                >{isDeviceZipCodeEdit ? 'save' : 'edit'}
                        </button>
                    </div>

                    <div className="DeviceAttributeMainBox">
                        <div className="UpdateIntervalTitleText">
                            Device Update Interval
                        </div>
                        {
                            isUpdateIntervalEdit ?
                            
                            <input  
                                className="UpdateIntervalValueInputEditing" 
                                id="POLL_VALUE" 
                                type='number' 
                                min={1} max={99}
                                value={newDeviceUpdateIntervalValue}
                                defaultValue={newDeviceUpdateIntervalValue}
                                onChange={(e) => {setNewDeviceUpdateIntervalValue(parseInt(e.target.value))}}>
                            </input> : 
                            <div className="UpdateIntervalValueInput">
                                <div className="UpdateIntervalValueInputText">
                                    {newDeviceUpdateIntervalValue}
                                </div>
                            </div>
                        }
                        {
                            isUpdateIntervalEdit ?
                            <select 
                                className="UpdateIntervalLabelSelectorEditing" 
                                id="POLL_LABEL"
                                onChange={(e) => {
                                    const timeFrameLabelString = (e.target as HTMLSelectElement).value;
                                    setNewDeviceUpdateIntervalLabel(timeFrameLabelString);
                                }}>
                                    <option value={'SEC'} selected={newDeviceUpdateIntervalLabel === 'SEC'}>
                                        {`Second(s)`}
                                    </option>
                                    <option value={'MIN'} selected={newDeviceUpdateIntervalLabel === 'MIN'}>
                                        {`Minute(s)`}
                                    </option>
                                    <option value={'HOUR'} selected={newDeviceUpdateIntervalLabel === 'HOUR'}>
                                        {`Hour(s)`}
                                    </option>
                                    <option value={'DAY'} selected={newDeviceUpdateIntervalLabel === 'DAY'}>
                                        {`Day(s)`}
                                    </option>
                                    <option value={'WEEK'} selected={newDeviceUpdateIntervalLabel === 'WEEK'}>
                                        {`Week(s)`}
                                    </option>
                                    <option value={'MONTH'} selected={newDeviceUpdateIntervalLabel === 'MONTH'}>
                                        {`Month(s)`}
                                    </option>
                            </select> : 
                            <div className="UpdateIntervalLabelSelector">
                                <div className="UpdateIntervalLabelSelectorText">
                                    {
                                        (newDeviceUpdateIntervalLabel === 'SEC') ? `Second(s)` :
                                        (newDeviceUpdateIntervalLabel === 'MIN') ? `Minute(s)` :
                                        (newDeviceUpdateIntervalLabel === 'HOUR') ? `Hour(s)` :
                                        (newDeviceUpdateIntervalLabel === 'DAY') ? `Day(s)` :
                                        (newDeviceUpdateIntervalLabel === 'WEEK') ? `Week(s)` :
                                        (newDeviceUpdateIntervalLabel === 'MONTH') ? `Month(s)` : ''
                                    }
                                </div>
                            </div>
                        }
                        
                        <button 
                            className="deviceInfoEditButton"
                            onClick={(e) => {
                                    convertUpdateTime(newDeviceUpdateIntervalValue, newDeviceUpdateIntervalLabel);
                                    setIsUpdateIntervalEdit(!isUpdateIntervalEdit);
                                }}
                                >{isUpdateIntervalEdit ? 'save' : 'edit'}
                        </button>
                    </div>
                </div>
            }
            {
                deviceUpdateAttempt ? null :
                    isDeletingDevice ? 
                    <div className="DeviceConfigButtonBox">
                        <div className="DeviceConfigDeleteText">
                            Confirm Device Deletion
                        </div>
                        <button 
                            className="DeviceConfigDeleteButton"
                            onClick={(e) => {
                                setIsDeletingDevice(false);
                                updateDevice(true);
                            }}>
                                Yes, Delete Device.
                        </button>
                        <button 
                            className="DeviceConfigDeleteButton"
                            onClick={(e) => {
                                setIsDeletingDevice(false);
                            }}>
                                No, Cancel.
                        </button>
                    </div>  :
                    <div className="DeviceConfigButtonBox">
                        <button 
                            className="DeviceConfigUpdateButton"
                            onClick={(e) => {
                                updateDevice(false);
                            }}>
                                Save and Update Device
                        </button>
                        <button 
                            className="DeviceConfigCancelButton"
                            onClick={(e) => {
                                configureDevice(false, true)
                            }}>
                                Cancel All Changes and Exit
                        </button>
                        <button 
                            className="DeviceConfigDeleteButton"
                            onClick={(e) => {
                                setIsDeletingDevice(true);
                            }}>
                                Delete Device
                        </button>
                    </div> 
            }
        </div>
    )
}

export default ConfigureDevice;