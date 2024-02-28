import React, { useEffect, useState, useCallback } from "react";
import { SensorDataType } from "../interfaces";
import { proxyURL } from "../variables.js";
import axios from "axios";

interface GraphProps {
    selectedSensorID: number;
    selectedChannelID: number;
    selectedTimeFrame: number;
    isLoggingOut: boolean;
}

const Graph: React.FC<GraphProps> = ({
    selectedSensorID,
    selectedChannelID,
    selectedTimeFrame,
    isLoggingOut
}: GraphProps) => {
    const [sensorData, setSensorData] = useState([] as SensorDataType[]);
    const [currentTime, setCurrentTime] = useState(Date.now());

    console.log('SELECTED TIME FRAME', selectedTimeFrame)
    // console.log('time frame? - ', Object.keys(timeFrameConstants));

    const getSensorData = useCallback(async(selectedSensorID: number) => {
        let tempSensorData: SensorDataType[] = [];
        if(selectedSensorID !== 0 && !isLoggingOut) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Sensor/${selectedSensorID}/SensorDatas`,
            })
                .then(function (response) {
                    // console.log('response', response);
                    // setSensorData(response.data);
                    tempSensorData = response.data;
                    // console.log('SENSORS FROM AXIOS', tempSensorData)
                }).catch(error => {
                    console.log(error);
                })
    
            // console.log('SET DATA', tempSensorData)
            setSensorData(tempSensorData);
        }

    }, [isLoggingOut])

    useEffect(() => {
        getSensorData(selectedSensorID);
    }, [selectedSensorID, getSensorData]);

    return (
        <div className="MainVisualizationBox">
            {
                (((selectedSensorID === 0) || selectedSensorID === undefined) ||
                    ((selectedChannelID === 0) || selectedChannelID === undefined)) ? 
                    null :
                <div className="mainGraphBox">
                    <table>
                        <thead>
                            <tr>
                                <th>Sensord Data ID</th>
                                <th>Data Value</th>
                                <th>Data Unit</th>
                                <th>Time Recorded</th>
                                <th>Sensor ID</th>
                                <th>Channel ID</th>
                            </tr>
                        </thead>
                        
                        <tbody>
                            {
                                sensorData.map((sensorData, i) => {
                                    // console.log('sensorData.timeRecorded', sensorData.timeRecorded)
                                    // console.log('Date.parse(sensorData.timeRecorded)', Date.parse(sensorData.timeRecorded));
                                    if((sensorData.channelID === selectedChannelID) && 
                                        ((Date.parse(sensorData.timeRecorded) > (currentTime - selectedTimeFrame) ||
                                            (selectedTimeFrame === 0)))
                                    ) {
                                        return (
                                            <tr key={i}>
                                                <td>{i+1}</td>
                                                <td>{sensorData.dataValue}</td>
                                                <td>{sensorData.dataUnit}</td>
                                                <td>{sensorData.timeRecorded}</td>
                                                <td>{sensorData.sensorID}</td>
                                                <td>{sensorData.channelID}</td>
                                            </tr>
                                        )    
                                    } else {
                                        return null;
                                    }
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

    // if(sensorData.length > 0) {
    //     console.log('sensorData',sensorData);
    //     const tempDate = sensorData[0].timeRecorded;
    //     const currentTime = Date.now();
    //     const test = new Date();
    //     const lastweek = new Date(test.getTime() - timeFrameConstants.WEEK);
    //     // console.log('CURRENT DAY - ', test);
    //     // console.log('LAST WEEK', lastweek);
    //     // const paresed = currentTime;
    //     // const paresedDay = currentTime - timeFrameConstants.DAY;
    //     // const paresedWeek = currentTime - timeFrameConstants.WEEK;
    //     // const paresedMonth = currentTime - timeFrameConstants.MONTH;
    //     // const paresed6Months = currentTime - timeFrameConstants.SIX_MONTHS;
    //     // const paresedYear = currentTime - timeFrameConstants.YEAR;
    //     // // const paresed = Date.parse(tempDate);
    //     // // const paresedDay = Date.parse(tempDate) - timeFrameConstants.DAY;
    //     // // const paresedWeek = Date.parse(tempDate) - timeFrameConstants.WEEK;
    //     // // const paresedMonth = Date.parse(tempDate) - timeFrameConstants.MONTH;
    //     // // const paresed6Months = Date.parse(tempDate) - timeFrameConstants.SIX_MONTHS;
    //     // // const paresedYear = Date.parse(tempDate) - timeFrameConstants.YEAR;


    //     // console.log("temp date", currentTime);
    //     // console.log('temp date epoch -', paresed);
    //     // console.log('temp date again? - ', new Date(paresed));
    //     // console.log('paresedDay', new Date(paresedDay));
    //     // console.log('paresedWeek', new Date(paresedWeek));
    //     // console.log('paresedMonth', new Date(paresedMonth));
    //     // console.log('paresed6Months', new Date(paresed6Months));
    //     // console.log('paresedYear', new Date(paresedYear));
    //     // sensorData.forEach((sensorData, i) => {
    //     //     if(sensorData.channelID === selectedChannelID) {
    //     //         return (
    //     //             <tr key={i}>
    //     //                 <td>{i+1}</td>
    //     //                 <td>{sensorData.dataValue}</td>
    //     //                 <td>{sensorData.dataUnit}</td>
    //     //                 <td>{sensorData.timeRecorded}</td>
    //     //                 <td>{sensorData.sensorID}</td>
    //     //                 <td>{sensorData.channelID}</td>
    //     //             </tr>
    //     //         )    
    //     //     }})
    // }


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
