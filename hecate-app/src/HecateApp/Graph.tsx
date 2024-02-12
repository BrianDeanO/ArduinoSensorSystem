import React, { useEffect, useState, useCallback } from "react";
import { SensorDataType, SelectedSensorData } from "../interfaces";
import { localStorageTitles, proxyURL } from "../Variables";
import axios from "axios";

interface GraphProps {
    selectedSensorID: number;
    selectedTimeFrame: string;
}

const Graph: React.FC<GraphProps> = ({
    selectedSensorID,
    selectedTimeFrame
}: GraphProps) => {
    const [sensorData, setSensorData] = useState([] as SensorDataType[])


    const getSensorData = useCallback(async(selectedSensorID: number) => {
        let tempSensorData: SensorDataType[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/Sensor/${selectedSensorID}/SensorDatas`,
        })
            .then(function (response) {
                console.log('response', response);
                // setSensorData(response.data);
                tempSensorData = response.data;
                console.log('SENSORS FROM AXIOS', tempSensorData)
            }).catch(error => {
                console.log(error);
            })

        /*
            NEED TO PERFORM TIME FRAME SELECTION HERE
        */

        console.log('SET DATA', tempSensorData)
        setSensorData(tempSensorData);

    }, [])

    useEffect(() => {
        getSensorData(selectedSensorID);
    }, [selectedSensorID, getSensorData])

    console.log('GRAPH - SELECTED SENSOR ID - ', selectedSensorID)

    return (
        <div className="MainVisualizationBox">
            GRAPH
            {`\nSensor ID - ${selectedSensorID || 0}`}
            {`\nSelected Time Frame: ${selectedTimeFrame}`}
            {
                ((selectedSensorID === 0) || selectedSensorID === undefined) ? null :
                <div>
                    <table>
                        <thead>
                            <tr>
                                <th>Sensord Data ID</th>
                                <th>Data Value</th>
                                <th>Data Unit</th>
                                <th>Time Recorded</th>
                                <th>Sensor ID</th>
                            </tr>
                        </thead>
                        
                        <tbody>
                            {
                                // Object.keys(sensorDataTable).map((sensorData, i) => {
                                    sensorData.map((sensorData, i) => {
                                    return (
                                        <tr key={i}>
                                            <td>{i+1}</td>
                                            <td>{sensorData.dataValue}</td>
                                            <td>{sensorData.dataUnit}</td>
                                            <td>{sensorData.timeRecorded}</td>
                                            <td>{sensorData.sensorID}</td>
                                        </tr>
                                    )    
                                })
                            }
                        </tbody>
                    </table>
                </div>
            }
        </div>  
    )
}

export default Graph;

export const sensorDataTable = [
    {
        channelID: 1, 
        dataValue: 13, 
        dataUnit: "F", 
        timeRecorded: "2024-01-01T18:42:27.069Z",
        sensorID: 1
    },
    {
        channelID: 1, 
        dataValue: 14, 
        dataUnit: "F", 
        timeRecorded: "2024-01-02T18:42:27.069Z",
        sensorID: 1
    },
    { 
        channelID: 2, 
        dataValue: 15, 
        dataUnit: "F", 
        timeRecorded: "2024-01-03T18:42:27.069Z",
        sensorID: 1
    },
    {
        channelID: 2, 
        dataValue: 12, 
        dataUnit: "F", 
        timeRecorded: "2024-01-04T18:42:27.069Z",
        sensorID: 1
    },
    { 
        channelID: 2, 
        dataValue: 16, 
        dataUnit: "F", 
        timeRecorded: "2024-01-05T18:42:27.069Z",
        sensorID: 1
    },
    {
        channelID: 1, 
        dataValue: 19, 
        dataUnit: "F", 
        timeRecorded: "2024-01-06T18:42:27.069Z",
        sensorID: 1
    },
    { 
        channelID: 1, 
        dataValue: 1361, 
        dataUnit: "W/m^2", 
        timeRecorded: "2024-01-07T18:42:27.069Z",
        sensorID: 2
    },
    { 
        channelID: 1, 
        dataValue: 1459,
        dataUnit: "W/m^2",
        timeRecorded: "2024-01-08T18:42:27.069Z",
        sensorID: 2
    },
    { 
        channelID: 1, 
        dataValue: 30,
        dataUnit: "atm",
        timeRecorded: "2024-01-09T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 2, 
        dataValue: 31,
        dataUnit: "atm", 
        timeRecorded: "2024-01-10T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 1, 
        dataValue: 33,
        dataUnit: "atm",
        timeRecorded: "2024-01-11T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 2, 
        dataValue: 29,
        dataUnit: "atm", 
        timeRecorded: "2024-01-12T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 1, 
        dataValue: 31, 
        dataUnit: "atm", 
        timeRecorded: "2024-01-13T18:42:27.069Z",
        sensorID: 3
    }
] as SensorDataType[]