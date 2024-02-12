import React, { useCallback, useEffect, useState } from "react";
import { SensorType, SensorDTOType, DeviceType } from "../interfaces";
import { channelCountArray, proxyURL } from "../Variables";
import axios from "axios";

interface NewSensorProps {
    addSensor: (addingSensor: boolean) => void;
}

const NewSensor: React.FC<NewSensorProps> = (
    {
    addSensor
}: NewSensorProps
) => {  
    const [devices, setDevices] = useState([] as DeviceType[])

    const [newSensor, setNewSensor] = useState({} as SensorType);
    const [newSensorName, setNewSensorName] = useState('');
    const [newSensorType, setNewSensorType] = useState('');
    const [newSensorChannelCount, setNewSensorChannelCount] = useState(0);
    const [deviceIDForeignKey, setDeviceIDForeignKey] = useState(0);

    const [sensorPostAttempt, setSensorPostAttempt] = useState(false);
    const [sensorPostedCorrectly, setsensorPostedCorrectly] = useState(false);
    const [postError, setPostError] = useState('false');

    async function postSensor() {
        // let sensorPostedCorrectly = false;
        console.log('SENSOR IN POST SENSOR');

        console.log("newSensorName", newSensorName);
        console.log("newSensorType", newSensorType);
        console.log("newSensorChannelCount", newSensorChannelCount);
        console.log("deviceIDForeignKey", deviceIDForeignKey);

        await axios.post(`${proxyURL}/api/Sensor?deviceId=${deviceIDForeignKey}`, {
            sensorName: newSensorName,
            sensorType: newSensorType,
            channelCount: newSensorChannelCount
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            console.log('response', response);
            setsensorPostedCorrectly(true);
        }).catch(function (error) {
            console.log(error);
            setPostError(error.code)
        });

        if(sensorPostedCorrectly) {
            console.log('POSTED CORRECTLY');
            addSensor(false);
        }
        
        setSensorPostAttempt(true);

        console.log('AFTER POST DEVICE');
    }

    async function getDevices() {
        await axios({
            method: 'get',
            url: `${proxyURL}/api/Device`,
        })
            .then(function (response) {
                // console.log('response', response);
                setDevices(response.data);
                // console.log('DEVICES FROM AXIOS', tempDevices)
            }).catch(error => {
                console.log(error);
            })
    }

    useEffect(() => {
        getDevices();
    }, []);


    return (
        <div className="NewSensorMainBox">
                {
                    sensorPostAttempt ? 
                    <div className="NewSensorUserSelectionMainBox">
                        <div className="NewSensorErrorText">
                            {sensorPostedCorrectly ? 
                                <div className="NewSensorErrorSubText">   
                                    <span>
                                       { `Sensor: ${newSensorName}`}
                                    </span>
                                    <span> Added </span>
                                </div> : 
                                <div className="NewSensorErrorSubText">
                                    <span> { `${postError}.`} </span>
                                    <span> Sensor Not Added </span>
                                </div>                                
                            }
                        </div>
                        <span 
                            className="NewSensorExitButton"
                            onClick={(e) => {
                                addSensor(false);
                            }}
                        >
                            X
                        </span> 
                    </div> :

                    <div className="newSensorTextEntryFieldsBox">
                        <div className="sensorNameEntryBox">
                            <div className="newSensorTitleText">
                                Sensor Name
                            </div>

                            <textarea
                                className="newSensorInputTextArea"
                                value={newSensorName}
                                id={'DEVICE_NAME'}
                                onChange={(e) => {setNewSensorName(e.target.value.toString());}}
                                cols={1}
                                rows={1}></textarea>
                        </div>
                        <div className="sensorTypeEntryBox">
                            <div className="newSensorTitleText">
                                Sensor Type
                            </div>
                            <textarea
                                className="newSensorInputTextArea"
                                value={newSensorType}
                                id={'DEVICE_TYPE'}
                                onChange={(e) => {setNewSensorType(e.target.value.toString());}}
                                cols={1}
                                rows={1}></textarea>
                        </div>
                        <div className="sensorChannelCountEntryBox">
                            <div className="newSensorTitleText">
                                Channel Count
                            </div>
                            <select 
                                className="sensorTypeSelector" 
                                id="sensorType"
                                onClick={(e) => {
                                    console.log('e.target.value', (e.target as HTMLSelectElement).value)
                                    const tempStringID = (e.target as HTMLSelectElement).value;
                                    const tempCountID = parseInt((tempStringID != null) ? tempStringID : "");
                                    setNewSensorChannelCount(tempCountID);
                                }}>
                                    {
                                        channelCountArray.map((countValue, index) => {
                                            return (
                                                <option value={countValue} key={index}>
                                                    {countValue}
                                                </option>
                                            )
                                        })
                                    }
                            </select>
                            {/* <textarea
                                className="newSensorInputTextArea"
                                value={newSensorChannelCount}
                                id={'DEVICE_ZIPCODE'}
                                onChange={(e) => {setNewSensorChannelCount(e.target.value.toString());}}
                                cols={1}
                                rows={1}></textarea> */}
                        </div>
                        <div className="newSensorDeviceSelectionBox">
                            <div className="newSensorTitleText">
                                DEVICE SELECTION
                            </div>
                            <select 
                                className="sensorTypeSelector" 
                                id="sensorType"
                                onClick={(e) => {
                                    console.log('e.target.value', (e.target as HTMLSelectElement).value)
                                    const tempStringID = (e.target as HTMLSelectElement).value;
                                    const tempSensorID = parseInt((tempStringID != null) ? tempStringID : "");
                                    setDeviceIDForeignKey(tempSensorID);
                                }}>
                                    {
                                        devices.map((device, index) => {
                                            // if(sensor.sensorID !== sensors[index]) return null;
                                            return (
                                                <option value={device.deviceID} key={index}>
                                                    {device.deviceName}
                                                </option>
                                            )
                                        })
                                    }
                            </select>
                        </div>
                    </div>
                }
                {
                    sensorPostAttempt ? null :
                    <div className="SensorSubmitAndCancelMainBox">
                        <button 
                            className="mainButton"
                            onClick={(e) => {
                                console.log("newSensorName",newSensorName)
                                console.log("newSensorType", newSensorType)
                                console.log("newSensorChannelCount", newSensorChannelCount)
                                postSensor();
                            }}>
                                Submit
                        </button>
                        <button 
                            className="mainButton"
                            onClick={(e) => {
                                addSensor(false)
                            }}>
                                Cancel
                        </button>
                    </div>
                }
        </div>
    )
}

export default NewSensor;