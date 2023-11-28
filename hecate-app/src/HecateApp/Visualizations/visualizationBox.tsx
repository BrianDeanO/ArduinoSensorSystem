import React, { useEffect, useState, useCallback } from "react";
import GraphContainer, { data } from "./graphContainer";
import { sensorDataTable, variables, sensorTable } from "../../Variables";

export type sensorData = {
    data_ID: number;
    sensor_ID: number;
    channel_ID: number;
    data_value: number;
    data_unit: string;
    time_recorded: string;
}

export type sensorInfoType = {
    sensor_ID: number;
    sensor_name: string;
    sensor_type: string;
    channel_count: number;
}

const VisualizationBox = (
    sensorInfo: string
) => {
    // const [data, setData] = useState([]);
    const [selectedChannel, setSelectedChannel] = useState(0);
    let dataArray: sensorData[] = [];
    let minValue;
    let maxValue;
    let channels: number[] = [];
    let dataType = '';
    let dataLabel = '';

    console.log('sensorDataTable', sensorDataTable)

    //@ts-ignore

    if(sensorInfo.sensorInfo) {
        //@ts-ignore
        const cArray = new Array(sensorInfo.sensorInfo.channel_count);
        console.log('carray ', cArray)
        sensorDataTable.forEach((entry, index) => {
            //@ts-ignore
            if(entry.sensor_ID === sensorInfo.sensorInfo.sensor_ID) {
                //@ts-ignore
                dataType = sensorInfo.sensorInfo.sensor_type;
                dataArray.push(entry);
            }
        })
        //@ts-ignore
        for(let i = 0; i <= sensorInfo.sensorInfo.channel_count; i++) {
            cArray[i] = i + 1;
            // channels.push(i);
        }
        console.log('carray 2', cArray)
        channels = cArray;
    }
    //@ts-ignore

    console.log('sensor info', sensorInfo.sensorInfo);
    console.log('dataArray', dataArray)

    // function refreshList() {
    //     //@ts-ignore
    //     fetch(variables.API_URL + sensorInfo.sensor_type)
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
    //                 dataLabel = d.data_unit;
    //                 minValue = d.data_value;
    //                 maxValue = d.data_value;
    //             }
    //             if(minValue >= d.data_value) {minValue = d.data_value};
    //             if(maxValue <= d.data_value) {maxValue = d.data_value};
    //             return d.data_value
    //         } 
    //         else {
    //             if(d.channel_ID === selectedChannel) {
    //                 if(index === 0) {
    //                     dataLabel = d.data_unit;
    //                     minValue = d.data_value;
    //                     maxValue = d.data_value;
    //                 }
    //                 if(minValue >= d.data_value) {minValue = d.data_value};
    //                 if(maxValue <= d.data_value) {maxValue = d.data_value};
    //                 return d.data_value
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
    //             dataLabel = d.data_unit;
    //             minValue = d.data_value;
    //             maxValue = d.data_value;
    //         }
    //         if(minValue >= d.data_value) {minValue = d.data_value};
    //         if(maxValue <= d.data_value) {maxValue = d.data_value};
    //         return d.data_value
    //     } 
    //     else {
    //         if(d.channel_ID === selectedChannel) {
    //             if(index === 0) {
    //                 dataLabel = d.data_unit;
    //                 minValue = d.data_value;
    //                 maxValue = d.data_value;
    //             }
    //             if(minValue >= d.data_value) {minValue = d.data_value};
    //             if(maxValue <= d.data_value) {maxValue = d.data_value};
    //             return d.data_value
    //         }
    //     }
    // }, [selectedChannel]);

    // console.log('test', test)

    const dataValues = dataArray ? dataArray.map((d, index) => {
        console.log('selectedChannel', selectedChannel)
        if(selectedChannel === 0) {
            console.log('ALLLLLL', d);
            if(index === 0) {
                dataLabel = d.data_unit;
                minValue = d.data_value;
                maxValue = d.data_value;
            }
            if(minValue >= d.data_value) {minValue = d.data_value};
            if(maxValue <= d.data_value) {maxValue = d.data_value};
            return d.data_value
        } 
        else {
            console.log('d', d)
            if(d.channel_ID === selectedChannel) {
                if(index === 0) {
                    dataLabel = d.data_unit;
                    minValue = d.data_value;
                    maxValue = d.data_value;
                }
                if(minValue >= d.data_value) {minValue = d.data_value};
                if(maxValue <= d.data_value) {maxValue = d.data_value};
                return d.data_value
            } else {
                return null;
            }
        }
    }) : [];

    const dataTimes = dataArray ? dataArray.map((d) => {
        if(selectedChannel === 0) {
            return d.time_recorded
        }
        else {
            if(d.channel_ID === selectedChannel) {
                return d.time_recorded
            } else {
                return null;
            }
        }
    }) : [];
    
    console.log('dataValues', dataValues);
    console.log('channels', channels)

    return (
        <div className="mainVisualizationBox">
            <div className="channelTypeSelectorBox">
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
                            channels.map((channel) => {
                                return (
                                    <option value={channel} key={channel}>
                                        {channel}
                                    </option>
                                )
                                
                            })
                        }
                </select>
            </div>
            <GraphContainer 
                values={dataValues} 
                times={dataTimes} 
                dataLabel={dataLabel} 
                //@ts-ignore
                dataTitle={`${dataType} (${dataLabel})`} 
                maxValue={maxValue}
                minValue={minValue}
            /> 
        </div>
    )
}

export default VisualizationBox;