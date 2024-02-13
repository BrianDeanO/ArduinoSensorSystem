import React, { useEffect, useState, useCallback } from "react";
import Graph from "./Graph.tsx";
import { DeviceType, SensorType } from "../interfaces";
import { timeFrameConstants, proxyURL } from "../Variables.js";
import axios from "axios";

interface SelectorProps {
    selectedDeviceID: number;
    selectSensor: (selectedSensorID: number, resetChannelID: boolean, resetTimeFrame: boolean) => void;
    selectTimeFrame: (selectedTimeFrame: string) => void;
    selectedSensorID: number;
    // getSensorOverride: boolean
    addSensor: (addingSensor: boolean) => void;
    selectChannel: (selectedChannelID: number) => void;
}

const Selectors: React.FC<SelectorProps> = ({
    selectedDeviceID, 
    selectSensor,
    selectedSensorID,
    selectTimeFrame,
    selectChannel,
    addSensor,
}: SelectorProps) => {
    const [sensors, setSensors] = useState([] as SensorType[]);
    const [channels, setChannels] = useState([] as number[]);
    // const [selectedChannels, setSelectedChannels] = useState([] as number[]);

    /*
        NEED TO FIGURE OUT HOW TO GET THE CHANNEL COUNT FROM THE SELECTED CHANNEL ID
        OBIVOUSLY JUST USE IT TO GET SENSOR. THEN USE setChannels
    */

    const getSensors = useCallback(async(selectedDeviceID: number) => {

        let tempSensors: SensorType[] = [];
        let tempChannels: number[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/Device/${selectedDeviceID}/Sensors`,
        })
        .then(function (response) {
            // console.log('response', response);
            // setSensors(response.data);
            tempSensors = response.data;
            // console.log('SENSORS FROM AXIOS', tempSensors)
        }).catch(error => {
            console.log(error);
        })

        setSensors(tempSensors);

        tempSensors.forEach((sensor, i) => {
            tempChannels.push(sensor.channelCount);
        })
        console.log('tempChannels', tempChannels)

        tempChannels.forEach((sensor, i) => {
            console.log('CHANNEL ??????????', i)
        })

        setChannels(tempChannels);
        
        selectSensor(selectedSensorID || 0, false, false);

    }, [selectSensor, selectedSensorID])

    useEffect(() => {
        // if(getSensorOverride) {
        // }
        getSensors(selectedDeviceID);
        // setSelectedChannels(new Array(channels[selectedSensorID - 1]));
    }, [ selectedDeviceID, getSensors ])

    // console.log('CURRENTLY SELECTED SNEOSRS ID', selectedSensorID)
    console.log('SENSORS - SELECTED SENSOR ID - ', selectedSensorID)

    // console.log('Array(channels[selectedSensorID - 1])', Array(channels[selectedSensorID - 1]));



    return (
        <div className="SubSelectorsBox">
            {/* <div className="AddSensorBox">
                <span 
                    className="AddSensorButton"
                    onClick={(e) => {
                        addSensor(true);
                }}>
                    Add New Sensor
                </span>   
            </div> */}
            <div className="sensorTypeSelectorBox">
                <div className="sensorTypeSelectorText">Sensor</div>
                <select 
                    className="sensorTypeSelector" 
                    id="sensorType"
                    onChange={(e) => {
                        const tempStringID = (e.target as HTMLSelectElement).value;
                        console.log('ON CHNACING SENSORS')
                        if(tempStringID === '---') {
                            selectSensor(0, true, true);
                        } else {
                            const tempSensorID = parseInt((tempStringID != null) ? tempStringID : "");
                            selectSensor(tempSensorID, true, true);
                            sensors.forEach((sensor, i) => {
                                if(sensor.sensorID === tempSensorID) {
                                    setChannels(new Array(sensor.channelCount));
                                }
                            })
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
            {/* {
                // (selectedSensorID !== 0) ? (
                (channels.length > 0) ?
                    <div className="channelTypeSelectorBox">
                        <div className="channelTypeSelectorText">Sensor Channel</div>
                        <select 
                            className="channelTypeSelector" 
                            id="channel"
                            onChange={(e) => {
                                const tempStringID = (e.target as HTMLSelectElement).value;
                                if(tempStringID === '---') {
                                    selectChannel(0);
                                } else {
                                    const tempChannelID = parseInt((tempStringID != null) ? tempStringID : "");
                                    console.log('SELECTINC CAHNNEL', tempChannelID)
                                    selectChannel(tempChannelID);
                                }
                            }}>
                                
                                <option value={'---'} selected={(channels.length === 0) || !(channels)}>---</option>
                                {
                                    (selectedSensorID !== 0) ? 
                                        channels.map((channel, i) => {
                                            return(
                                                <option value={i+1} key={i} >
                                                    {i + 1}
                                                </option>
                                            )
                                        })
                                        : null
                                }
                        </select>
                    </div> : 
                    <div className="channelTypeSelectorBox">
                        <div className="channelTypeSelectorText">Sensor Channel</div>
                        <div className="channelTypeSelectorText">No Channels</div>
                    </div>
            } */}

            {
                <div className="channelTypeSelectorBox">
                <div className="channelTypeSelectorText">Sensor Channel</div>
                <select 
                    className="channelTypeSelector" 
                    id="channel"
                    onChange={(e) => {
                        const tempStringID = (e.target as HTMLSelectElement).value;
                        if(tempStringID === '---') {
                            selectChannel(0);
                        } else {
                            const tempChannelID = parseInt((tempStringID != null) ? tempStringID : "");
                            console.log('SELECTINC CAHNNEL', tempChannelID)
                            selectChannel(tempChannelID);
                        }
                    }}>
                        
                        <option value={'---'} selected={(channels.length === 0) || !(channels)}>---</option>
                        {
                            (selectedSensorID !== 0) ? 
                                channels.map((channel, i) => {
                                    return(
                                        <option value={i+1} key={i} >
                                            {i + 1}
                                        </option>
                                    )
                                })
                                : null
                        }
                </select>
            </div> 
            }
            {/* <div className="timeTypeSelectorBox">
                <div className="timeTypeSelectorText">Time Frame</div>
                <select 
                    className="timeTypeSelector" 
                    id="sensorType"
                    defaultValue={'Lifetime'}
                    onChange={(e) => {
                        const timeFrameString = (e.target as HTMLSelectElement).value;
                        selectTimeFrame(timeFrameString);
                    }}> */}
                        {/* timeFrameConstants can be used here to subtract times? */}
                        {/* <option value={'Past Day'}>Past Day</option>
                        <option value={'Past Week'}>Past Week</option>
                        <option value={'Past Month'}>Past Month</option>
                        <option value={'Past Six Months'}>Past Six Months</option>
                        <option value={'Lifetime'}>Lifetime</option>
                </select>
            </div> */}
        </div>
    )
}

export default Selectors;