import React, { useEffect, useState, useCallback } from "react";
import { SensorDataType } from "../interfaces";
import { proxyURL, timeFrameConstants, monthsOfTheYear, repeatCountLimits } from "../variables.js";
import axios from "axios";
// import {
//     Chart,
//     CategoryScale,
//     LinearScale,
//     PointElement,
//     LineElement,
//     Title,
//     Tooltip,
//     Legend,
//     defaults,
//     TitleOptions,
//     LineOptions
// } from 'chart.js';
import {Chart} from 'chart.js/auto'
import { Line } from 'react-chartjs-2';

// Chart.register(CategoryScale);
// Chart.register(linear);
// Chart.register(CategoryScale);
// Chart.register(CategoryScale);
// Chart.register(CategoryScale);
// Chart.register(CategoryScale);

interface GraphProps {
    selectedSensorID: number;
    selectedChannelID: number;
    selectedTimeFrame: number;
    isLoggingOut: boolean;
}

Chart.defaults.maintainAspectRatio = false;
Chart.defaults.responsive = true;

// defaults.plugins.title.display = true;
// defaults.plugins.title.align =  "start";
// // defaults.plugins.title.font = 20;
// defaults.plugins.title.color = "black";


const Graph: React.FC<GraphProps> = ({
    selectedSensorID,
    selectedChannelID,
    selectedTimeFrame,
    isLoggingOut
}: GraphProps) => {
    const [allSensorData, setAllSensorData] = useState([] as SensorDataType[]);
    const [sensorData, setSensorData] = useState([] as SensorDataType[]);
    const [dataValues, setDataValues] = useState([] as number[]);
    const [dataTimes, setDataTimes] = useState([] as string[]);
    // const [currentTime, setCurrentTime] = useState(Date.now());
    const [timeLabels, setTimeLabels] = useState([] as string[])

    const getSensorData = useCallback(async(selectedSensorID: number) => {
        let tempAllSensorData: SensorDataType[] = [];
        let tempSensorData: SensorDataType[] = [];
        let tempTimeLabels: string[] = [];
        let timeLabelCounts: number[] = [];
        let repeatCount = 0;
        const currentTime = Date.now();

        console.log('getting sensor data', selectedTimeFrame)
        if(selectedSensorID !== 0 && !isLoggingOut) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/Sensor/${selectedSensorID}/SensorDatas`,
            })
                .then(function (response) {
                    tempAllSensorData = response.data;
                }).catch(error => {
                    console.log(error);
                })
                
            // setAllSensorData(tempAllSensorData);

            if((((selectedSensorID !== 0) && selectedSensorID !== undefined) ||
                ((selectedChannelID !== 0) && selectedChannelID !== undefined))) {
                    tempAllSensorData.forEach((data) => {
                        if((data.channelID === selectedChannelID) && 
                            ((Date.parse(data.timeRecorded) > (currentTime - selectedTimeFrame) ||
                                (selectedTimeFrame === 0)))
                        ) {
                            tempSensorData.push(data);
                        }
                    })
                
                    setSensorData(tempSensorData);

                    tempSensorData.forEach((data) => {
                        let timeLabel: string;
                        let repeatCountLimit: number;
                        if(data.channelID === selectedChannelID) {
                            // console.log('selectedChannelID', selectedChannelID)
                            // console.log('time recorded',  data.timeRecorded);
                            const tempFullDate = new Date(data.timeRecorded);
                            // console.log('tempFullDate', tempFullDate);
                            let tempSeconds: string = tempFullDate.getSeconds().toString();
                            let tempMinutes: string = tempFullDate.getMinutes().toString();
                            let tempHours: string = tempFullDate.getHours().toString();
                            let tempDate: string = tempFullDate.getDate().toString();
                            let tempMonth: string = (tempFullDate.getMonth() + 1).toString();

                            // console.log('seconds', (tempFullDate).getSeconds())
                            // console.log('minutes', tempFullDate.getMinutes())
                            // console.log('hours', tempFullDate.getHours())
                            // console.log('day', tempFullDate.getDate())
                            // console.log('month', tempFullDate.getMonth())
                            
                            if(tempSeconds.length === 1) {
                                tempSeconds = `0${tempSeconds}`
                            }
                            if(tempMinutes.length === 1) {
                                tempMinutes = `0${tempMinutes}`
                            }
                            if(tempHours.length === 1) {
                                tempHours = `0${tempHours}`
                            }
                            if(tempDate.length === 1) {
                                tempDate = `0${tempDate}`
                            }
                            if(tempMonth.length === 1) {
                                tempMonth = `0${tempMonth}`
                            }

                            switch(selectedTimeFrame) {
                                case timeFrameConstants.DAY: {
                                    timeLabel = `${tempHours}:${tempMinutes}:${tempSeconds}`;
                                    repeatCountLimit = repeatCountLimits.DAY;
                                    break;
                                }
                                case timeFrameConstants.WEEK: 
                                case timeFrameConstants.MONTH: {
                                    timeLabel = `${tempMonth}/${tempDate}`;
                                    repeatCountLimit = repeatCountLimits.WEEK_MONTH;
                                    break;
                                }
                                case timeFrameConstants.SIX_MONTHS:
                                case timeFrameConstants.YEAR: {
                                    timeLabel = `${monthsOfTheYear[tempFullDate.getMonth()]}`;
                                    repeatCountLimit = repeatCountLimits.HALF_AND_FULL_YEAR;
                                    break;
                                }
                                default:
                                    timeLabel = `${monthsOfTheYear[tempFullDate.getMonth()]}. ${tempFullDate.getFullYear()}`;
                                    repeatCountLimit = repeatCountLimits.LIFETIME;
                                    break;
                            }

                            console.log('timeLabel', timeLabel)
                            console.log('timeLabels', tempTimeLabels)
                            console.log('timeLabelCounts', timeLabelCounts)
                        
                            let labelRecorded = false;
                            let repeatLabel = false;

                            if(tempTimeLabels.length === 0) {
                                console.log('initial push into time leabesl', timeLabelCounts)
                                tempTimeLabels.push(timeLabel);
                                timeLabelCounts.push(0);
                            } else {
                                tempTimeLabels.forEach((label, i) => {
                                    if(label === timeLabel) {
                                        labelRecorded = true;
                                        // console.log('timeLabelCounts[i]', timeLabelCounts[i])
                                        // timeLabelCounts[i] = timeLabelCounts[i] + 1;
                                        // if(timeLabelCounts[i] > 3) {
                                        //     repeatLabel = true;
                                        //     timeLabelCounts[i] = 0;
                                        // }
                                        return;
                                    }
                                })
                                if(!labelRecorded) {
                                    repeatCount = 0;
                                    console.log('unique label push', timeLabel)
                                    tempTimeLabels.push(timeLabel);
                                    // timeLabelCounts.push(0);
                                    
                                    console.log('timeLabels', tempTimeLabels)
                                    console.log('timeLabelCounts', timeLabelCounts)
                                } 
                                // else if(repeatLabel) {
                                //     repeatCount++;
                                //     console.log('unique label push', timeLabel)
                                //     tempTimeLabels.push(timeLabel);
                                    
                                //     console.log('timeLabels', tempTimeLabels)
                                //     console.log('timeLabelCounts', timeLabelCounts)
                                //     repeatLabel = false;
                                // }
                                
                                else {
                                    repeatCount++;
                                    if(repeatCount > repeatCountLimit) {
                                        tempTimeLabels.push(timeLabel);
                                        repeatCount = 0;
                                    } else {
                                        tempTimeLabels.push('');
                                    }
                                }
                            }
                        }
                    })

                    setTimeLabels(tempTimeLabels);

                    // console.log('FINAL TIME LABELS', tempTimeLabels)
                    // tempSensorData.forEach((data) => {
                    //     let labelRecorded = false;
                    //     if(tempTimeLabels.length === 0) {
                    //         tempTimeLabels.push(data.timeRecorded);
                    //     } 
                        
                    //     else {
                    //         tempTimeLabels.forEach((label) => {
                    //             if(label === data.timeRecorded) {
                    //                 labelRecorded = true;
                    //                 return;
                    //             }
                    //         })
                    //         if(!labelRecorded) {
                    //             tempTimeLabels.push(data.timeRecorded);
                    //         }
                    //     }
                    // })
            }
        }
    }, [isLoggingOut, selectedChannelID, selectedTimeFrame])

    useEffect(() => {
        getSensorData(selectedSensorID);
        console.log('USING THIS EFFECT')
    }, [selectedSensorID, getSensorData, selectedChannelID, selectedTimeFrame]);

    return (
        <div className="MainVisualizationBox">
            {
                // (((selectedSensorID === 0) || selectedSensorID === undefined) ||
                // ((selectedChannelID === 0) || selectedChannelID === undefined)) ? 
                (sensorData.length === 0) ? null :
                    <Line
                        data={{
                                labels: timeLabels,
                                datasets: [
                                {
                                    label: sensorData[0].dataUnit,
                                    data: sensorData.map((data) => {
                                        return ((data.channelID === selectedChannelID) ? 
                                            data.dataValue : null
                                        ) 
                                    }),
                                    // Could do  different colors based on sensor type
                                    borderColor: 'rgb(0, 128, 0)',
                                    backgroundColor: 'rgba(0, 128, 0, 0.856)',
                                }
                                ],
                        }}
                        options = {{
                            responsive: true,
                            plugins: {
                                title: {
                                    text: `Sensor ${selectedSensorID} Data`
                                },
                                legend: {
                                    position: 'top',
                                  },
                            },
                            
                            scales: {
                                x: {
                                    display: true,
                                    title: {
                                        display: true,
                                        text: 'Time Recorded',
                                        color: '#000000',
                                        font: {
                                          size: 20,
                                          weight: 'bold',
                                          lineHeight: 1.2,
                                        },
                                    },
                                    // ticks: {
                                    //     autoSkip: true,
                                    //     maxTicksLimit: 
                                    //         (selectedTimeFrame === timeFrameConstants.DAY) ? 24 :
                                    //         (selectedTimeFrame === timeFrameConstants.WEEK) ? 7 :
                                    //         (selectedTimeFrame === timeFrameConstants.MONTH) ? 30 :
                                    //         (selectedTimeFrame === timeFrameConstants.SIX_MONTHS) ? 6 : 12 
                                    // }
                                },
                                y: {
                                    display: true,
                                    title: {
                                        display: true,
                                        text: 'Data Value',
                                        color: '#000000',
                                        font: {
                                          size: 20,
                                          weight: 'bold',
                                          lineHeight: 1.2,
                                        },
                                    }
                                }
                            }
                        }}
                    />
            }
            


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
                                        ((Date.parse(sensorData.timeRecorded) > (Date.now() - selectedTimeFrame) ||
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

                    
                    {/* { 
                        values ? 
                        <Line options = {options(values.minValue, values.maxValue, values.dataLabel, values.dataTitle)} data={data(values.values, values.times)}/> : 
                        null
                    } */}
                </div>
            }
        </div>  
    )
}

export default Graph;



                                // sensorData.map((data) => {
                                //     if(data.channelID === selectedChannelID) {
                                //         const tempFullDate = new Date(data.timeRecorded);
                                //         // console.log('tempFullDate', tempFullDate);
                                //         let tempSeconds: string = tempFullDate.getSeconds().toString();
                                //         let tempMinutes: string = tempFullDate.getMinutes().toString();
                                //         let tempHours: string = tempFullDate.getHours().toString();
                                //         let tempDate: string = tempFullDate.getDate().toString();
                                //         let tempMonth: string = (tempFullDate.getMonth() + 1).toString();
                                        
                                //         if(tempSeconds.length === 1) {
                                //             tempSeconds = `0${tempSeconds}`
                                //         }
                                //         if(tempMinutes.length === 1) {
                                //             tempMinutes = `0${tempMinutes}`
                                //         }
                                //         if(tempHours.length === 1) {
                                //             tempHours = `0${tempHours}`
                                //         }
                                //         if(tempDate.length === 1) {
                                //             tempDate = `0${tempDate}`
                                //         }
                                //         if(tempMonth.length === 1) {
                                //             tempMonth = `0${tempMonth}`
                                //         }

                                //         switch(selectedTimeFrame) {
                                //             case timeFrameConstants.DAY: {
                                //                 return `${tempHours}:${tempMinutes}:${tempSeconds}`;
                                //             }
                                //             case timeFrameConstants.WEEK: 
                                //             case timeFrameConstants.MONTH: {
                                //                 return `${tempMonth}/${tempDate}`;
                                //             }
                                //             case timeFrameConstants.SIX_MONTHS:
                                //             case timeFrameConstants.YEAR: {
                                //                 return `${monthsOfTheYear[tempFullDate.getMonth()]}`;
                                //             }
                                //             default:
                                //                 return `${monthsOfTheYear[tempFullDate.getMonth()]}, ${tempFullDate.getFullYear()}`;
                                //         }
                                //     } else {
                                //         return null;
                                //     }
                                // }),
