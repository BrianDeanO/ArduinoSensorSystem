import React, { useEffect, useState, } from "react";
import { SensorDataType, SelectedSensorData } from "../interfaces";
import { localStorageTitles } from "../Variables";

interface GraphProps {
    selectedSensorID: number;
}

const Graph: React.FC<GraphProps> = ({
    selectedSensorID
}: GraphProps) => {

    // const sensorDataJSON = localStorage.getItem(localStorageTitles.selectedSensorData);
    // const selectedSensorData: SelectedSensorData = (sensorDataJSON !== null) ? JSON.parse(sensorDataJSON) : {};

    // useEffect(() => {
    //     /**********
    //       USE AXIOS TO GET SENSOR DATA TO DISPLAY

    //     *************/
    //     localStorage.setItem(localStorageTitles.selectedSensorData, JSON.stringify({
    //         selectedSensorID: selectedSensorID,
    //         // selectedSensorData: selectedSensorData
    //     }));
    // }, [selectedSensorID, selectedSensorData])


    useEffect(() => {
        /**********
          USE AXIOS TO GET SENSOR DATA TO DISPLAY

        *************/
        
    }, [selectedSensorID])


    return (
        <div className="MainVisualizationBox">
            GRAPH
            {`\nSensor ID - ${selectedSensorID || 0}`}
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
                            </tr>
                        </thead>
                        
                        <tbody>
                            {
                                Object.keys(sensorDataTable).map((sensorData, i) => {
                                    return (
                                        <tr key={i}>
                                            <td>{i}</td>
                                            <td>{sensorDataTable[i].dataValue}</td>
                                            <td>{sensorDataTable[i].dataUnit}</td>
                                            <td>{sensorDataTable[i].timeRecorded}</td>
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