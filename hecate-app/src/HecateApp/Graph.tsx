import React, { useEffect, useState, useCallback } from "react";
import { SensorDataType } from "../interfaces";
import { proxyURL, timeFrameConstants, monthsOfTheYear, repeatCountLimits } from "../variables.js";
import axios from "axios";
import {Chart} from 'chart.js/auto'
import { Line } from 'react-chartjs-2';

interface GraphProps {
    selectedSensorID: number;
    selectedChannelID: number;
    selectedTimeFrame: number;
    isLoggingOut: boolean;
}

Chart.defaults.maintainAspectRatio = false;
Chart.defaults.responsive = true;

const Graph: React.FC<GraphProps> = ({
    selectedSensorID,
    selectedChannelID,
    selectedTimeFrame,
    isLoggingOut
}: GraphProps) => {
    const [sensorData, setSensorData] = useState([] as SensorDataType[]);
    const [timeLabels, setTimeLabels] = useState([] as string[])

    const getSensorData = useCallback(async(selectedSensorID: number) => {
        let tempAllSensorData: SensorDataType[] = [];
        let tempSensorData: SensorDataType[] = [];
        let tempTimeLabels: string[] = [];
        let repeatCount = 0;
        const currentTime = Date.now();

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
                            const tempFullDate = new Date(data.timeRecorded);
                            let labelRecorded = false;

                            let tempSeconds: string = tempFullDate.getSeconds().toString();
                            let tempMinutes: string = tempFullDate.getMinutes().toString();
                            let tempHours: string = tempFullDate.getHours().toString();
                            let tempDate: string = tempFullDate.getDate().toString();
                            let tempMonth: string = (tempFullDate.getMonth() + 1).toString();
                            
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

                            if(tempTimeLabels.length === 0) {
                                tempTimeLabels.push(timeLabel);
                            } else {
                                tempTimeLabels.forEach((label) => {
                                    if(label === timeLabel) {
                                        labelRecorded = true;
                                        return;
                                    }
                                })

                                if(!labelRecorded) {
                                    repeatCount = 0;
                                    tempTimeLabels.push(timeLabel);
                                } else {
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
            }
        }
    }, [isLoggingOut, selectedChannelID, selectedTimeFrame])

    useEffect(() => {
        getSensorData(selectedSensorID);
    }, [selectedSensorID, getSensorData, selectedChannelID, selectedTimeFrame]);

    return (
        <div className="MainVisualizationBox">
            {
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
        </div>  
    )
}

export default Graph;