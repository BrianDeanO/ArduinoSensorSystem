import React, { useCallback, useEffect, useState } from "react";
import { SensorType, SensorDTOType, DeviceType, PollingTimeIntervals } from "../../interfaces";
import { channelCountArray, proxyURL, deviceTable, timeFrameConstants, pollingTimeIntervalLabels } from "../../variables.js";
import axios from "axios";

interface ConfigureDeviceProps {
    configureDevice: (configuringDevice: boolean, resetDevices: boolean) => void;
    selectedDeviceID: number;
}

const ConfigureDevice: React.FC<ConfigureDeviceProps> = (
    {
    configureDevice,
    selectedDeviceID
}: ConfigureDeviceProps
) => {  

    // console.log('deviceTable 1', deviceTable[1]);

    const [newDeviceName, setNewDeviceName] = useState('');
    const [isDeviceNameEdit, setIsDeviceNameEdit ] = useState(false);

    const [newDeviceIdent, setNewDeviceIdent] = useState('');
    const [isDeviceIdentEdit, setIsDeviceIdentEdit] = useState(false);

    const [newDeviceType, setNewDeviceType] = useState('');
    const [isDeviceTypeEdit, setIsNewDeviceTypeEdit] = useState(false);

    const [newDeviceZipCode, setNewDeviceZipCode] = useState('');
    const [isDeviceZipCodeEdit, setIsDeviceZipCodeEdit] = useState(false);

    const [newDevicePollingIntervalValue, setNewDevicePollingIntervalValue] = useState(1);
    // const [newDevicePollingIntervalValue, setNewDevicePollingIntervalValue] = useState('');    

    const [newDevicePollingIntervalLabel, setNewDevicePollingIntervalLabel] = useState('');
    // const [newDevicePollingIntervalValue, setNewDevicePollingIntervalValue] = useState('');    

    const [newDevicePollingInterval, setNewDevicePollingInterval] = useState('');

    const [isPollingIntervalEdit, setIsPollingIntervalEdit] = useState(false);


    const [deviceUpdateAttempt, setDeviceUpdateAttempt] = useState(false);

    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);
    const [postError, setPostError] = useState(false);
    const [userDevicesAddedCorrectly, setUserDevicesAddedCorrectly] = useState(false);
    const [userSelectionArray, setUserSelectionArray] = useState([] as number[]);
    // const [tempDevice, setTempDevice] = useState({} as DeviceType);

    const getDevice = useCallback(async(deviceID: number) => {
        console.log('GET DEVICE', deviceID);
        let tempDevice: DeviceType;
        await axios({
            method: 'get',
            url: `${proxyURL}/api/Device/${deviceID}`,
        })
            .then(function (response) {
                console.log('RESPONSE', response);
                tempDevice = response.data;
                setNewDeviceName(tempDevice.deviceName);
                setNewDeviceIdent(tempDevice.deviceIdent);
                setNewDeviceType(tempDevice.deviceType);
                setNewDeviceZipCode(tempDevice.deviceZipCode);

                parsePollingInterval(parseInt(tempDevice.devicePollingInterval));

                console.log('AFTER AXIOSXIOS')
            }).catch(error => {
                console.log(error);
            })

    }, []);

    async function parsePollingInterval(devicePollingInterval: number) {
        console.log('devicePollingInterval', devicePollingInterval);
        let pollingValue: number = 1;
        let pollingLabel: string = 'DAY';

        // devicePollingInterval = (1000 * 40);

        if((devicePollingInterval % timeFrameConstants.MONTH) === 0) {
            pollingValue = devicePollingInterval / timeFrameConstants.MONTH;
            pollingLabel = 'MONTH';
        }

        else if((devicePollingInterval % timeFrameConstants.WEEK) === 0) {
            pollingValue = devicePollingInterval / timeFrameConstants.WEEK;
            pollingLabel = 'WEEK';
        }

        else if((devicePollingInterval % timeFrameConstants.DAY) === 0) {
            pollingValue = devicePollingInterval / timeFrameConstants.DAY;
            pollingLabel = 'DAY';
        }

        else if((devicePollingInterval % (1000 * 60 * 60)) === 0) {
            pollingValue = devicePollingInterval / (1000 * 60 * 60);
            pollingLabel = 'HOUR';
        }

        else if((devicePollingInterval % (1000 * 60)) === 0) {
            pollingValue = devicePollingInterval / (1000 * 60);
            pollingLabel = 'MIN';
        }

        else if((devicePollingInterval % 1000) === 0) {
            pollingValue = devicePollingInterval / 1000;
            pollingLabel = 'SEC';
        }

        setNewDevicePollingIntervalValue(pollingValue);
        setNewDevicePollingIntervalLabel(pollingLabel);

        // setNewDevicePollingIntervalValue(11);
        // setNewDevicePollingIntervalLabel('DAY');
    }

    async function convertPollingTime(intervalValue: number, intervalLabel: string) {
        console.log('inter VALUE', intervalValue)
        console.log('inter LAEBL', intervalLabel)

        switch(intervalLabel) {
            case 'MONTH': 
                setNewDevicePollingInterval((intervalValue * timeFrameConstants.MONTH).toString());
                break;

            case 'WEEK': 
                setNewDevicePollingInterval((intervalValue * timeFrameConstants.WEEK).toString());
                break;

            case 'DAY': 
                setNewDevicePollingInterval((intervalValue * timeFrameConstants.DAY).toString());
                break;

            case 'HOUR': 
                setNewDevicePollingInterval((intervalValue * (1000 * 60 * 60)).toString());
                break;

            case 'MIN':
                setNewDevicePollingInterval((intervalValue * (1000 * 60)).toString());
                break;

            case 'SEC': 
                setNewDevicePollingInterval((intervalValue * 1000).toString());
                break;

            default:
                break;
        }
    }

    useEffect(() => {
        getDevice(selectedDeviceID);
    }, [selectedDeviceID, getDevice])


    async function updateDevice() {

        // // let updatedCorrectly = false;
        // console.log('DEVICE IN POST DEVICE');
        // console.log('NAME', newDeviceName);
        // console.log('IDENT', newDeviceIdent);
        // console.log('TYPE', newDeviceType);
        // console.log('ZIP CODE', newDeviceZipCode);
        // const convertedPollingInterval = convertPollingTime(newDevicePollingIntervalValue, newDevicePollingIntervalLabel);
        console.log('newDevicePollingInterval', newDevicePollingInterval);

        setDeviceUpdateAttempt(true);
        
        await axios.put(`${proxyURL}/api/Device/${selectedDeviceID}`, {
            deviceID: selectedDeviceID,
            deviceIdent: newDeviceIdent,
            deviceName: newDeviceName,
            deviceType: newDeviceType,
            deviceZipCode: newDeviceZipCode,
            devicePollingInterval: newDevicePollingInterval,
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

        setNewDeviceName("");
        setNewDeviceIdent("");
        setNewDeviceType("");
        setNewDeviceZipCode("");
        setNewDevicePollingIntervalValue(1);
        setNewDevicePollingIntervalLabel('');

        console.log('AFTER POST DEVICE');
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
                                    {`Successfully Updated Device`}
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
                            Device Identifier
                        </div>
                        {isDeviceIdentEdit ? 
                            <textarea
                                className="deviceTextArea"
                                value={newDeviceIdent}
                                id={'IDENT'}
                                onChange={(e) => {setNewDeviceIdent(e.target.value.toString());}}
                                spellCheck={false}
                                cols={1}
                                rows={1}>{newDeviceIdent}</textarea> : 
                            <div className="configDeviceStandardTextBox">
                                <div className="configDeviceStandardText">
                                    {newDeviceIdent}
                                </div>
                            </div>
                        }
                        <button 
                            className="deviceInfoEditButton"
                            onClick={(e) => {
                                    setIsDeviceIdentEdit(!isDeviceIdentEdit);
                                    if(isDeviceIdentEdit) {
                                        console.log('DEVICE IDENT',(document.getElementById('IDENT') as HTMLInputElement).value )
                                    }
                                }}
                                >{isDeviceIdentEdit ? 'save' : 'edit'}
                        </button>
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
                                    // if(isDeviceIdentEdit)
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
                        <div className="PollingIntervalTitleText">
                            Device Polling Interval
                        </div>
                        {/* {abilityBoxEdit ? 
                            <input  className="AbilityScoreInput" 
                                    id="STR" 
                                    type='number' 
                                    min={3} max={20}
                                    defaultValue={strengthScore}></input> :
                            <div className="AbilityScore"> {strengthScore} </div>
                        } */}
                        {
                            isPollingIntervalEdit ?
                            
                            <input  
                                className="PollingIntervalValueInputEditing" 
                                id="POLL_VALUE" 
                                type='number' 
                                min={1} max={99}
                                value={newDevicePollingIntervalValue}
                                defaultValue={newDevicePollingIntervalValue}
                                onChange={(e) => {setNewDevicePollingIntervalValue(parseInt(e.target.value))}}>
                            </input> : 
                            <div className="PollingIntervalValueInput">
                                <div className="PollingIntervalValueInputText">
                                    {newDevicePollingIntervalValue}
                                </div>
                            </div>
                        }
                        {
                            isPollingIntervalEdit ?
                            <select 
                                className="PollingIntervalLabelSelectorEditing" 
                                id="POLL_LABEL"
                                onChange={(e) => {
                                    const timeFrameLabelString = (e.target as HTMLSelectElement).value;
                                    setNewDevicePollingIntervalLabel(timeFrameLabelString);
                                }}>
                                    <option value={'SEC'} selected={newDevicePollingIntervalLabel === 'SEC'}>
                                        {`Second(s)`}
                                    </option>
                                    <option value={'MIN'} selected={newDevicePollingIntervalLabel === 'MIN'}>
                                        {`Minute(s)`}
                                    </option>
                                    <option value={'HOUR'} selected={newDevicePollingIntervalLabel === 'HOUR'}>
                                        {`Hour(s)`}
                                    </option>
                                    <option value={'DAY'} selected={newDevicePollingIntervalLabel === 'DAY'}>
                                        {`Day(s)`}
                                    </option>
                                    <option value={'WEEK'} selected={newDevicePollingIntervalLabel === 'WEEK'}>
                                        {`Week(s)`}
                                    </option>
                                    <option value={'MONTH'} selected={newDevicePollingIntervalLabel === 'MONTH'}>
                                        {`Month(s)`}
                                    </option>
                            </select> : 
                            <div className="PollingIntervalLabelSelector">
                                <div className="PollingIntervalLabelSelectorText">
                                    {
                                        (newDevicePollingIntervalLabel === 'SEC') ? `Second(s)` :
                                        (newDevicePollingIntervalLabel === 'MIN') ? `Minute(s)` :
                                        (newDevicePollingIntervalLabel === 'HOUR') ? `Hour(s)` :
                                        (newDevicePollingIntervalLabel === 'DAY') ? `Day(s)` :
                                        (newDevicePollingIntervalLabel === 'WEEK') ? `Week(s)` :
                                        (newDevicePollingIntervalLabel === 'MONTH') ? `Month(s)` : ''
                                    }
                                </div>
                            </div>
                        }
                        
                        <button 
                            className="deviceInfoEditButton"
                            onClick={(e) => {
                                    convertPollingTime(newDevicePollingIntervalValue, newDevicePollingIntervalLabel);
                                    setIsPollingIntervalEdit(!isPollingIntervalEdit);
                                }}
                                >{isPollingIntervalEdit ? 'save' : 'edit'}
                        </button>
                    </div>
                </div>
            }
            {
                deviceUpdateAttempt ? null :
                    <div className="DeviceConfigButtonBox">
                        <button 
                            className="DeviceConfigUpdateButton"
                            onClick={(e) => {
                                updateDevice();
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
                    </div>
            }
        </div>
    )
}

export default ConfigureDevice;