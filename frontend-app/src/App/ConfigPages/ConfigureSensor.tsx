import React, { useCallback, useEffect, useState } from "react";
import { SensorType, DeviceType } from "../../interfaces.js";
import { proxyURL } from "../../variables.js";
import axios from "axios";

interface ConfigureSensorProps {
    configureSensor: ((configuringSensor: boolean) => void);
    selectedSensorID: number;
}

const ConfigureSensor: React.FC<ConfigureSensorProps> = (
    {
    configureSensor,
    selectedSensorID
}: ConfigureSensorProps
) => {  
    const [devices, setDevices] = useState([] as DeviceType[]);

    const [newSensorName, setNewSensorName] = useState('');
    const [isSensorNameEdit, setIsSensorNameEdit ] = useState(false);

    const [newSensorIdent, setNewSensorIdent] = useState('');

    const [newSensorType, setNewSensorType] = useState('');
    const [isSensorTypeEdit, setIsNewSensorTypeEdit] = useState(false);

    const [newSensorChannelCount, setNewSensorChannelCount] = useState(0);

    const [newSensorDeviceID, setNewSensorDeviceID] = useState(0);

    const [isDeletingSensor, setIsDeletingSensor] = useState(false);

    const [sensorUpdateAttempt, setSensorUpdateAttempt] = useState(false);
    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);
    const [updateDeleteMessage, setUpdateDeleteMessage] = useState('');
    const [postError, setPostError] = useState('');

    const getDevices = useCallback(async() => {
        let tempDevices: DeviceType[] = [];

        if(selectedSensorID !== 0) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Device`,
            })
                .then(function (response) {
                    tempDevices = response.data;
                }).catch(error => {
                    console.log(error);
                })
    
            setDevices(tempDevices);
        }

    }, [selectedSensorID]);

    const getSensor = useCallback(async(sensorID: number) => {
        let tempSensor: SensorType;

        if(sensorID !== 0) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Sensor/${sensorID}`,
            })
                .then(function (response) {
                    tempSensor = response.data;

                    setNewSensorName(tempSensor.sensorName);
                    setNewSensorIdent(tempSensor.sensorIdent);
                    setNewSensorType(tempSensor.sensorType);
                    setNewSensorChannelCount(tempSensor.channelCount);
                    setNewSensorDeviceID(tempSensor.deviceID);
                }).catch(error => {
                    console.log(error);
                })
        }
    }, []);

    useEffect(() => {
        getDevices();
        getSensor(selectedSensorID);
    }, [getDevices, getSensor, selectedSensorID])

    async function updateSensor(deletedSensor: boolean) {
        await axios.put(`${proxyURL}/api/Sensor/${selectedSensorID}`, {
            sensorID: selectedSensorID,
            sensorIdent: newSensorIdent,
            sensorName: newSensorName,
            sensorType: newSensorType,
            channelCount: newSensorChannelCount,
            deviceID: newSensorDeviceID,
            sensorIsDeleted: deletedSensor
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            setUpdatedCorrectly(true);
            setUpdateDeleteMessage(deletedSensor 
                    ? 'Successfully Deleted Sensor.' : 'Successfully Updated Sensor.');
        }).catch(function (error) {
            console.log(error);
            setPostError(error.code)
        });
        
        setSensorUpdateAttempt(true);
        setNewSensorType("");
        setNewSensorName("");
        setNewSensorChannelCount(0);
        setNewSensorDeviceID(0);
    }

    return (
        <div className="ConfigSensorMainBox">
            {  
                sensorUpdateAttempt ? 
                <div className="SensorUpdatedMainBox"> 
                    <div className="UpdateSensorErrorText">
                        {updatedCorrectly ?  
                            <div className="UpdateSensorErrorSubText">   
                                <span >
                                    {updateDeleteMessage}
                                </span>
                            </div> : 
                            <div className="UpdateSensorErrorSubText">
                                <span> { `${postError}`} </span>
                                <span> Unable to Updated Sensor </span>
                            </div>
                        }
                    </div>
                    <span 
                        className="UpdatedSensorExitButton"
                        onClick={(e) => {
                            configureSensor(false);
                        }}
                    >
                        X
                    </span>
                </div>
                :  
                <div className="ConfigSensorSubBox">
                    <div className="SensorAttributeMainBox">
                        <div className="ConfigSensorTitleText">
                            Sensor Identifier
                        </div>
                        <div className="ConfigSensorStandardTextBox">
                            <div className="ConfigSensorStandardText">
                                {newSensorIdent}
                            </div>
                        </div>
                    </div>

                    <div className="SensorAttributeMainBox">
                        <div className="ConfigSensorTitleText">
                            Sensor Name
                        </div>
                        {isSensorNameEdit ? 
                            <textarea
                                className="SensorTextArea"
                                value={newSensorName}
                                id={'NAME'}
                                onChange={(e) => {setNewSensorName(e.target.value.toString());}}
                                spellCheck={false}
                                cols={1}
                                rows={1}></textarea> : 
                            <div className="ConfigSensorStandardTextBox">
                                <div className="ConfigSensorStandardText">
                                    {newSensorName}
                                </div>
                            </div>
                        }
                        <button 
                            className="SensorInfoEditButton"
                            onClick={(e) => {
                                    setIsSensorNameEdit(!isSensorNameEdit);
                                }}
                                >{isSensorNameEdit ? 'save' : 'edit'}
                        </button>
                    </div>

                    <div className="SensorAttributeMainBox">
                        <div className="ConfigSensorTitleText">
                            Sensor Type
                        </div>
                        {isSensorTypeEdit ? 
                            <textarea
                                className="SensorTextArea"
                                value={newSensorType}
                                id={'TYPE'}
                                onChange={(e) => {setNewSensorType(e.target.value.toString());}}
                                spellCheck={false}
                                cols={1}
                                rows={1}></textarea> : 
                            <div className="ConfigSensorStandardTextBox">
                                <div className="ConfigSensorStandardText">
                                    {newSensorType}
                                </div>
                            </div>
                        }
                        <button 
                            className="SensorInfoEditButton"
                            onClick={(e) => {
                                    setIsNewSensorTypeEdit(!isSensorTypeEdit);
                                }}
                                >{isSensorTypeEdit ? 'save' : 'edit'}
                        </button>
                    </div>
                    
                    <div className="SensorAttributeMainBox">
                        <div className="ConfigSensorTitleText">
                            Channel Count
                        </div>
                        <div className="ConfigSensorStandardTextBox">
                            <div className="ConfigSensorStandardText">
                                {newSensorChannelCount}
                            </div>
                        </div>
                    </div>

                    <div className="SensorAttributeMainBox">
                        <div className="ConfigSensorTitleText">Associated Device</div>
                        <select 
                            className="SensorDeviceSelector" 
                            id="sensorDevice"
                            onChange={(e) => {
                                const tempStringID = (e.target as HTMLSelectElement).value;
                                const tempSensorDeviceID = parseInt((tempStringID != null) ? tempStringID : "");
                                setNewSensorDeviceID(tempSensorDeviceID);
                            }}>
                                {
                                    devices.map((device, index) => {
                                        return (
                                            <option value={device.deviceID} key={index} selected={device.deviceID === newSensorDeviceID}>
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
                sensorUpdateAttempt ? null :
                isDeletingSensor ? 
                    <div className="SensorConfigButtonBox">
                        <div className="SensorConfigDeleteText">
                            Confirm Sensor Deletion
                        </div>
                        <button 
                            className="SensorConfigDeleteButton"
                            onClick={(e) => {
                                setIsDeletingSensor(false);
                                updateSensor(true);
                            }}>
                                Yes, Delete Sensor.
                        </button>
                        <button 
                            className="SensorConfigDeleteButton"
                            onClick={(e) => {
                                setIsDeletingSensor(false);
                            }}>
                                No, Cancel.
                        </button>
                    </div> :
                    <div className="SensorConfigButtonBox">
                        <button 
                            className="SensorConfigUpdateButton"
                            onClick={(e) => {
                                updateSensor(false);
                            }}>
                                Save and Update Sensor
                        </button>
                        <button 
                            className="SensorConfigCancelButton"
                            onClick={(e) => {
                                configureSensor(false);
                            }}>
                                Cancel All Changes and Exit
                        </button>
                        <button 
                            className="SensorConfigDeleteButton"
                            onClick={(e) => {
                                setIsDeletingSensor(true);
                            }}>
                                Delete Sensor
                        </button>
                    </div>
            }
        </div>
    )
}

export default ConfigureSensor;