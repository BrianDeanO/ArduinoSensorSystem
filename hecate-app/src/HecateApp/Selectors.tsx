import React, { useEffect, useState, useCallback } from "react";
import { SensorDataType, SensorType, SensorChannels } from "../interfaces";
import { timeFrameConstants, proxyURL } from "../variables.js";
import axios from "axios";

interface SelectorProps {
    selectedDeviceID: number;
    selectSensor: ((selectedSensorID: number, resetChannelID: boolean, resetTimeFrame: boolean) => void);
    selectTimeFrame: ((selectedTimeFrame: number) => void);
    selectedTimeFrame: number;
    selectedSensorID: number;
    selectedChannelID: number;
    selectChannel: ((selectedChannelID: number) => void);
    isAdmin: boolean;
    configureSensor: ((configuringSensor: boolean) => void);
    isLoggingOut: boolean;
}

const Selectors: React.FC<SelectorProps> = ({
    selectedDeviceID, 
    selectSensor,
    selectedSensorID,
    selectedChannelID,
    selectTimeFrame,
    selectedTimeFrame,
    selectChannel,
    isAdmin,
    configureSensor,
    isLoggingOut
}: SelectorProps) => {
    const [sensors, setSensors] = useState([] as SensorType[]);
    const [sensorChannels, setSensorChannels] = useState([] as SensorChannels[]);

    const getSensors = useCallback(async(selectedDeviceID: number) => {
        let tempAllSensors: SensorType[] = [];
        let tempNonDeletedSensors: SensorType[] = [];
        let tempAllChannels: number[][] = [];
        let tempSensorChannels: SensorChannels[] = [];

        if(selectedDeviceID !== 0 && !isLoggingOut) {
            // Getting Sensors
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Device/${selectedDeviceID}/Sensors`,
            })
            .then(function (response) {
                tempAllSensors = response.data;
            }).catch(error => {
                console.log(error);
            })

            tempAllSensors.forEach((sensor) => {
                if(!sensor.sensorIsDeleted) {
                    tempNonDeletedSensors.push(sensor);
                }
            }) 
    
            setSensors(tempNonDeletedSensors);

            tempNonDeletedSensors.forEach((sensor, i) => {
                // tempSensorDatas = getSensorData(sensor.sensorID);
                
                // tempSensorDatas.

                // console.log('sensor', sensor)
                let tempChannels = [];
                // let tempSensorChannelsObj: SensorChannels;

                for(let j = 1; j <= sensor.channelCount; j++) {
                    tempChannels.push(j);
                }

                tempSensorChannels.push({
                    sensorID: sensor.sensorID,
                    channels: tempChannels
                } as SensorChannels);

                
                // console.log('tempChannels', tempChannels);
                tempAllChannels.push(tempChannels);
            });

            // console.log("tempSensorChannels", tempSensorChannels);
            setSensorChannels(tempSensorChannels);

            // setChannels(tempAllChannels);
            selectSensor(selectedSensorID || 0, false, false);
        } else {
            setSensors(tempAllSensors);
            // setChannels(tempAllChannels);
            setSensorChannels(tempSensorChannels);
        }
    }, [selectSensor, selectedSensorID, isLoggingOut]);

    // console.log('channel array', channels);

    useEffect(() => {
        if(isLoggingOut) {
            selectSensor(0, true, true);
        }
        getSensors(selectedDeviceID);
    }, [ selectedDeviceID, getSensors, isLoggingOut, selectSensor])

    return (
        <div className="SubSelectorsBox">
            <div className="sensorSelectorMainBox">
                <div className="sensorTypeSelectorBox">
                    <div className="sensorTypeSelectorText">Sensor</div>
                    <select 
                        className="sensorTypeSelector" 
                        id="sensorType"
                        onChange={(e) => {
                            const tempStringID = (e.target as HTMLSelectElement).value;
                            if(tempStringID === '---') {
                                selectSensor(0, true, true);
                            } else {
                                const tempSensorID = parseInt((tempStringID != null) ? tempStringID : "");
                                selectSensor(tempSensorID, true, true);
                            }
                        }}>
                            <option value={'---'} selected={selectedSensorID === 0}>---</option>
                            {
                                sensors.map((sensor, index) => {
                                    return (
                                        <option value={sensor.sensorID} key={index} selected={sensor.sensorID === selectedSensorID}>
                                            {sensor.sensorName}
                                        </option>
                                    )
                                })
                            }
                    </select>

                </div>
                {
                    (isAdmin && (selectedSensorID !== 0)) ? 
                    <span 
                        className="ConfigureSensorButton"
                        data-value={selectedDeviceID}
                        onClick={(e) => {
                            configureSensor(true);
                    }}>
                        Configure Sensor
                    </span> : null
                }
            </div>
            
            <div className="channelTypeSelectorBox">
                <div className="channelTypeSelectorText">Sensor Channel</div>
                <select 
                    className="channelTypeSelector" 
                    id="channel"
                    onChange={(e) => {
                        const tempStringID = (e.target as HTMLSelectElement).value;
                        if(tempStringID === '---') {
                            selectChannel(-1);
                        } else {
                            const tempChannelID = parseInt((tempStringID != null) ? tempStringID : "");
                            selectChannel(tempChannelID);
                        }
                    }}>
                        <option value={'---'} selected={selectedChannelID === -1}>---</option>
                        {
                            (selectedSensorID !== 0) ? 
                                sensorChannels.map((sensor) => {

                                    return (
                                        (sensor.sensorID === selectedSensorID) ?
                                            sensor.channels.map((channel, i) => {
                                                // console.log('channel', channel)
                                                return (
                                                    <option 
                                                        value={channel - 1} 
                                                        key={channel - 1} 
                                                        selected={(selectedChannelID === (i))}
                                                    >
                                                        {channel - 1}
                                                    </option>
                                                )
                                            }) : null
                                    )
                                })
                                : null
                        }
                </select>
            </div> 
            
            <div className="timeTypeSelectorBox">
                <div className="timeTypeSelectorText">Time Frame</div>
                <select 
                    className="timeTypeSelector" 
                    id="sensorType"
                    onChange={(e) => {
                        const timeFrameString = (e.target as HTMLSelectElement).value;
                        const tempTime = parseInt((timeFrameString != null) ? timeFrameString : "");
                        selectTimeFrame(tempTime);
                    }}>
                        <option value={timeFrameConstants.DAY} selected={selectedTimeFrame === timeFrameConstants.DAY}>
                            Past Day
                        </option>
                        <option value={timeFrameConstants.WEEK} selected={selectedTimeFrame === timeFrameConstants.WEEK}>
                            Past Week
                        </option>
                        <option value={timeFrameConstants.MONTH} selected={selectedTimeFrame === timeFrameConstants.MONTH}>
                            Past Month
                        </option>
                        <option value={timeFrameConstants.SIX_MONTHS} selected={selectedTimeFrame === timeFrameConstants.SIX_MONTHS}>
                            Past Six Months
                        </option>
                        <option value={timeFrameConstants.YEAR} selected={selectedTimeFrame === timeFrameConstants.YEAR}>
                            Past Year
                        </option>
                        <option 
                            value={0}
                            selected={selectedTimeFrame === 0}>
                            Lifetime
                        </option>
                </select>
            </div>
        </div>
    )
}

export default Selectors;