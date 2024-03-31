import React, { useCallback, useEffect, useState } from "react";
import { SensorType, DeviceType, SensorConfigType } from "../../interfaces.js";
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
    
    const [newSensorConfigs, setNewSensorConfigs] = useState([] as SensorConfigType[]);
    const [sensorConfigsIsEditArray, setSensorConfigsIsEditArray] = useState([] as boolean[]);
    const [sensorConfigIsBeingEdited, setSensorConfigIsBeingEdited] = useState(false);

    const [isDeletingSensor, setIsDeletingSensor] = useState(false);

    const [sensorUpdateAttempt, setSensorUpdateAttempt] = useState(false);
    const [updatedCorrectly, setUpdatedCorrectly] = useState(false);
    const [updateDeleteMessage, setUpdateDeleteMessage] = useState('');
    const [error, setError] = useState('');

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
        let tempSensorConfigs: SensorConfigType[];

        if(sensorID !== 0) {
            // Getting sensor
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
                });
            
            // Getting Sensor Configs
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Sensor/${sensorID}/SensorConfigs`,
            })
                .then(function (response) {
                    tempSensorConfigs = response.data;

                    setNewSensorConfigs(tempSensorConfigs);
                    setSensorConfigsIsEditArray(new Array(tempSensorConfigs.length).fill(false));
                }).catch(error => {
                    console.log(error);
                });            
        }
    }, []);

    useEffect(() => {
        getDevices();
        getSensor(selectedSensorID);
    }, [getDevices, getSensor, selectedSensorID])

    async function updateSensorConfig(sensorConfig: SensorConfigType): Promise<boolean> {
        let updatedCorrectly = false;

        await axios.put(`${proxyURL}/api/SensorConfig/${sensorConfig.sensorConfigID}`, {
            sensorConfigID: sensorConfig.sensorConfigID,
            sensorConfigKey: sensorConfig.sensorConfigKey,
            sensorConfigValue: sensorConfig.sensorConfigValue,
            sensorID: sensorConfig.sensorID
        }, {
            headers: {
                'Content-Type': 'application/json'
                }
        })
        .then(function (response) {
            updatedCorrectly =  true;
        }).catch(function (error) {
            setError(error);
            console.log(error);
        });

        return Promise.resolve(updatedCorrectly);
    }

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
            setError(error.code)
        });

        let originalSensorConfigs: SensorConfigType[] = [];

        // Getting Original Sensor Configs Again (due to previous unwanted behaivor)
        await axios({
            method: 'get',
            url: `${proxyURL}/api/Sensor/${selectedSensorID}/SensorConfigs`,
        })
            .then(function (response) {
                originalSensorConfigs = response.data;
            }).catch(error => {
                console.log(error);
            });         

        newSensorConfigs.forEach((sc, i) => {
            if(sc.sensorConfigValue !== originalSensorConfigs[i].sensorConfigValue) {
                updateSensorConfig(sc);
            }

            if(!updatedCorrectly) {
                return;
            }
        })
        
        setSensorUpdateAttempt(true);
        setNewSensorType("");
        setNewSensorName("");
        setNewSensorChannelCount(0);
        setNewSensorDeviceID(0);
        setNewSensorConfigs([]);
        setSensorConfigsIsEditArray([]);
    }

    function updateConfigValue(index: number, value: string) {
        newSensorConfigs.forEach((sc, i) => {
            if(i === index) {
                sc.sensorConfigValue = value;
                return;
            }
        });
        setNewSensorConfigs(newSensorConfigs);
    }

    function updateConfigValueEdits(index: number, value: boolean) {
        sensorConfigsIsEditArray.forEach((sc, i) => {
            if(i === index) {
                sc = value;
                return;
            }
        });
        setSensorConfigIsBeingEdited(!sensorConfigIsBeingEdited);
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
                                <span> { `${error}`} </span>
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
                           
                    {
                        (newSensorConfigs.length === 0) ? null :
                            <div className="SensorConfigMainBox">
                                <div className="SensorConfigTitleText">
                                    <div>Sensor Configuration Fields</div>
                                </div>
                                { 
                                    newSensorConfigs.map((sensorConfig, i) => {
                                        return (
                                            <div className={
                                                    (i === 0) ? 
                                                        ((i === (newSensorConfigs.length - 1)) ? "SensorConfigSingleSubBox" : "SensorConfigTopSubBox") : 
                                                        ((i === (newSensorConfigs.length - 1)) ? "SensorConfigBottomSubBox" : "SensorConfigSubBox")
                                                } 
                                                key={i}>
                                                <div className="ConfigSensorFieldTitleText">
                                                    {sensorConfig.sensorConfigKey}
                                                </div>
                                                {sensorConfigsIsEditArray[i] ? 
                                                    <textarea
                                                        className="SensorConfigFieldTextArea"
                                                        defaultValue={newSensorConfigs[i].sensorConfigValue}
                                                        id={`CONFIG_VALUE_${i}`}
                                                        onChange={(e) => {updateConfigValue(i, e.target.value.toString());}}
                                                        spellCheck={false}
                                                        cols={1}
                                                        rows={1}></textarea> : 
                                                    <div className="ConfigSensorFieldTextBox">
                                                        <div className="ConfigSensorFieldText">
                                                            {newSensorConfigs[i].sensorConfigValue}
                                                        </div>
                                                    </div>
                                                }
                                                <button 
                                                    className="SensorConfigValueEditButton"
                                                    onClick={(e) => {
                                                            sensorConfigsIsEditArray[i] = !sensorConfigsIsEditArray[i];
                                                            updateConfigValueEdits(i, !sensorConfigsIsEditArray[i]);
                                                        }}
                                                        >{sensorConfigsIsEditArray[i] ? 'save' : 'edit'}
                                                </button>
                                        </div>

                                            
                                        )
                                    })
                                }
                            </div>
                    }

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