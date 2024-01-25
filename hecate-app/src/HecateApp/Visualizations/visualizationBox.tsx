import React, { useEffect, useState, useCallback } from "react";
import GraphContainer, { data } from "./graphContainer";
import { sensorDataTable, sensorTable } from "../../Variables";

export type sensorDataType = {
    sensorDataID: number;
    sensorID: number;
    channelID: number;
    dataValue: number;
    dataUnit: string;
    timeRecorded: string;
}

export type sensorInfoType = {
    sensorID: number;
    sensorName: string;
    sensorType: string;
    channelCount: number;
}

export type deviceInfoType = {
    device_ID: number;
    deviceName: string;
    deviceType: string;
    zipCode: number;
}

interface VisualizationBoxProps {
    sensor: sensorInfoType;
    sensorData: sensorDataType[];
}

const VisualizationBox: React.FC<VisualizationBoxProps> = ({
    sensor,
    sensorData
}) => {
    // const [data, setData] = useState([]);
    const [selectedChannel, setSelectedChannel] = useState(0);
    let dataArray: sensorDataType[] = [];
    let minValue;
    let maxValue;
    // const ch = function() {
    //     const tempArray = [];
    //     for(let i = 0; i > sensor.channelCount; i++) {
    //         tempArray.push(i);
    //     }
    // }
    const channels = new Array(sensor.channelCount);
    for(let i = 0; i < sensor.channelCount; i++) {
        channels[i] = i+1;
    }
    let dataType = '';
    let dataLabel = '';

    console.log('FINAL SENSOR DATA ', sensorData)
    console.log('GINAL  SENSORS', sensor);

    //@ts-ignore

    // if(sensor) {
    //     const cArray = new Array(sensor.channelCount);
    //     console.log('carray ', cArray)
    //     Object.keys(sensorData).forEach(function(dataKey) {
    //         if(sensorData[dataKey].sensorID === sensor.sensorID) {
    //             dataType = sensor.sensorType;
    //             dataArray.push(sensorData[dataKey]);
    //         }
    //     });

    //     for(let i = 0; i <= sensor.channelCount; i++) {
    //         cArray[i] = i + 1;
    //         // channels.push(i);
    //     }
    //     console.log('carray 2', cArray)
    //     channels = cArray;
    // }
    //@ts-ignore

    console.log('sensor info', sensor);
    console.log('dataArray', dataArray)

    // function refreshList() {
    //     //@ts-ignore
    //     fetch(variables.API_URL + sensorInfo.sensorType)
    //     .then(response => response.json())
    //     .then(data => {
    //         setData(data)
    //     }).catch((error) => (console.log('Fetching Error - ', error)));
    // }

    // function refreshList() {
    //     fetch('api/todoitems')
    //     .then(response => response.json())
    //     .then(data => {
    //         setTData(data)
    //     });
    // }

    // useEffect(() => {
    //     refreshList();
    // }, []);

    // function f() {
    //     if(dataArray) {

    //     }
    //     dataArray ? dataArray.map((d, index) => {
    //         if(selectedChannel === 0) {
    //             if(index === 0) {
    //                 dataLabel = d.dataUnit;
    //                 minValue = d.dataValue;
    //                 maxValue = d.dataValue;
    //             }
    //             if(minValue >= d.dataValue) {minValue = d.dataValue};
    //             if(maxValue <= d.dataValue) {maxValue = d.dataValue};
    //             return d.dataValue
    //         } 
    //         else {
    //             if(d.channelID === selectedChannel) {
    //                 if(index === 0) {
    //                     dataLabel = d.dataUnit;
    //                     minValue = d.dataValue;
    //                     maxValue = d.dataValue;
    //                 }
    //                 if(minValue >= d.dataValue) {minValue = d.dataValue};
    //                 if(maxValue <= d.dataValue) {maxValue = d.dataValue};
    //                 return d.dataValue
    //             }
    //         }
    //     }) : return [];
    // }

    // const test1 = useEffect(() => {
    //     refreshList();
    // }, []);

    // console.log('data', data);

    // const test = useCallback((d, index) => {
    //     if(selectedChannel === 0) {
    //         if(index === 0) {
    //             dataLabel = d.dataUnit;
    //             minValue = d.dataValue;
    //             maxValue = d.dataValue;
    //         }
    //         if(minValue >= d.dataValue) {minValue = d.dataValue};
    //         if(maxValue <= d.dataValue) {maxValue = d.dataValue};
    //         return d.dataValue
    //     } 
    //     else {
    //         if(d.channelID === selectedChannel) {
    //             if(index === 0) {
    //                 dataLabel = d.dataUnit;
    //                 minValue = d.dataValue;
    //                 maxValue = d.dataValue;
    //             }
    //             if(minValue >= d.dataValue) {minValue = d.dataValue};
    //             if(maxValue <= d.dataValue) {maxValue = d.dataValue};
    //             return d.dataValue
    //         }
    //     }
    // }, [selectedChannel]);

    // console.log('test', test)

    // const dataValues = dataArray ? dataArray.map((d, index) => {
    //     console.log('selectedChannel', selectedChannel)
    //     if(selectedChannel === 0) {
    //         console.log('ALLLLLL', d);
    //         if(index === 0) {
    //             dataLabel = d.dataUnit;
    //             minValue = d.dataValue;
    //             maxValue = d.dataValue;
    //         }
    //         if(minValue >= d.dataValue) {minValue = d.dataValue};
    //         if(maxValue <= d.dataValue) {maxValue = d.dataValue};
    //         return d.dataValue
    //     } 
    //     else {
    //         console.log('d', d)
    //         if(d.channelID === selectedChannel) {
    //             if(index === 0) {
    //                 dataLabel = d.dataUnit;
    //                 minValue = d.dataValue;
    //                 maxValue = d.dataValue;
    //             }
    //             if(minValue >= d.dataValue) {minValue = d.dataValue};
    //             if(maxValue <= d.dataValue) {maxValue = d.dataValue};
    //             return d.dataValue
    //         } else {
    //             return null;
    //         }
    //     }
    // }) : [];

    // const dataTimes = dataArray ? dataArray.map((d) => {
    //     if(selectedChannel === 0) {
    //         return d.timeRecorded
    //     }
    //     else {
    //         if(d.channelID === selectedChannel) {
    //             return d.timeRecorded
    //         } else {
    //             return null;
    //         }
    //     }
    // }) : [];
    
    // console.log('dataValues', dataValues);
    console.log('channels', channels)

    return (
        <div className="mainVisualizationBox">
            {/* <div className="channelTypeSelectorBox">
                <div className="channelTypeSelectorText">Sensor Channel</div>
                <select 
                    className="channelTypeSelector" 
                    id="channel"
                    onClick={(e) => {
                        //@ts-ignore
                        setSelectedChannel(parseInt(e.target.value));
                    }}>
                        <option value={0}>All</option>
                        {
                            channels.map((channel, index) => {
                                return (
                                    <option value={channel} key={channel}>
                                        {channel}
                                    </option>
                                )
                                
                            })
                        }
                </select>
            </div> */}
            <div>
                {(sensor === null || sensor === undefined) ?
                    null :
                    <div>
                        <table>
                            <thead>
                                <tr>
                                    <th>Data ID</th>
                                    <th>Data Value</th>
                                    <th>Data Unit</th>
                                </tr>
                            </thead>
                            
                            <tbody>
                                {
                                     Object.keys(sensorData).map(function(dataKey) {
                                        return (
                                            <tr key={sensorData[dataKey].sensorDataID}>
                                                <td>{sensorData[dataKey].sensorDataID}</td>
                                                <td>{sensorData[dataKey].dataValue}</td>
                                                <td>{sensorData[dataKey].dataUnit}</td>
                                            </tr>
                                        )
                                        
                                    })
                                }
                            </tbody>
                        </table>
                    </div>
                }
            </div>
            {/* <GraphContainer 
                values={dataValues} 
                times={dataTimes} 
                dataLabel={dataLabel} 
                //@ts-ignore
                dataTitle={`${dataType} (${dataLabel})`} 
                maxValue={maxValue}
                minValue={minValue}
            />  */}
        </div>
    )
}

export default VisualizationBox;