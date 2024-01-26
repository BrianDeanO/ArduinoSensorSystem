import React, { useEffect, useState } from "react";
import { deviceInfoType, sensorDataType, sensorInfoType} from "./VisualizationBox.tsx";
import VisualizationBox from "./VisualizationBox.tsx";

interface DataSelectorsProps {
    selectedDevice: deviceInfoType;
    sensorDataTable: sensorDataType[];
    sensors: sensorInfoType[];
}

const DataSelectors: React.FC<DataSelectorsProps> = ({
    selectedDevice,
    sensorDataTable,
    sensors
}) => {
    const [selectedSensorInfo, setSelectedSensorInfo] = useState({} as sensorInfoType);
    const [sensorData, setSensorData] = useState<sensorDataType[]>([]);
    console.log('SELECTED DEVICE', selectedDevice);
    console.log('SENSOR TABLE', sensorDataTable);
    console.log('USER SECNORS', sensors);

    function getSelectedSensor(sensorID) {
        console.log('SEECTED ensorID', sensorID)
        let tempSensorDataArray: sensorDataType[] = [];
        sensors.forEach((sensorEntry, index) => {
            console.log('sennors', sensorEntry);
            if(sensorEntry.sensorID === parseInt(sensorID)) {
                console.log('PUSHING SENSOR', sensorEntry)
                setSelectedSensorInfo(sensorEntry);

                sensorDataTable.forEach((dataEntry, index) => {
                    console.log('DATeNTRY', dataEntry)
                    if(dataEntry.sensorID === sensorEntry.sensorID) {
                        console.log('PUSHING entry', dataEntry)
                        tempSensorDataArray.push(dataEntry);
                    }
                })
                console.log('AFTER DATA', tempSensorDataArray)
                setSensorData(tempSensorDataArray);

                return;
            }
        })
        // console.log('selectedSensorInfo', selectedSensorInfo)
        // if(Object.keys(selectedSensorInfo).length !== 0) {
        //     console.log('THEN SELECTED OGIN THROUGH DATABTELA');
        //     sensorDataTable.forEach((dataEntry, index) => {
        //         console.log('DATeNTRY', dataEntry)
        //         if(dataEntry.sensorID === selectedSensorInfo.sensorID) {
        //             console.log('PUSHING entry', dataEntry)
        //             tempSensorDataArray.push(dataEntry);
        //         }
        //     })
        //     console.log('AFTER DATA', tempSensorDataArray)
        //     setSensorData(tempSensorDataArray);
        // }
    }

    return (
        (Object.keys(selectedDevice).length === 0) ?
        <>
            <div className="MainSensorBox">
                 NO DEVICE SELECTED
            </div>
        </> :
        <div className="">
            <div className="MainSensorBox">
                <div className="sensorTypeSelectorBox">
                    <div className="sensorTypeSelectorText">Sensor</div>
                    <select 
                        className="sensorTypeSelector" 
                        id="sensorType"
                        onClick={(e) => {
                            console.log('e.target.value', (e.target as HTMLOptionElement).value)
                            // if(((e.target as HTMLOptionElement).value) === '---') {
                            //     setSelectedSensorInfo({});
                            // } else {
                            //     // setSelectedSensorInfo(sensorTable[e.target.value - 1]);
                            //     getSelectedSensor((e.target as HTMLOptionElement).getAttribute("value"))
                            // }
                            
                            getSelectedSensor((e.target as HTMLOptionElement).value)
                            console.log('selectedSensorInfo', selectedSensorInfo)
                        }}>
                            <option value={'---'}>---</option>
                            {
                                sensors.map((sensor, index) => {
                                    // if(sensor.sensorID !== sensors[index]) return null;
                                    return (
                                        <option value={sensor.sensorID} key={index}>
                                            {sensor.sensorName}
                                        </option>
                                    )
                                })
                            }
                            <option value={'ALL'}>All</option>
                    </select>
                </div>
                <div className="timeTypeSelectorBox">
                    <div className="timeTypeSelectorText">Time Frame</div>
                    <select 
                        className="timeTypeSelector" 
                        id="sensorType"
                        onClick={(e) => {
                            //@ts-ignore
                            setTimeType(e.target.value);
                        }}>
                            <option value={'---'}>---</option>
                            <option value={'Past Day'}>Past Day</option>
                            <option value={'Past Week'}>Past Week</option>
                            <option value={'Past Month'}>Past Month</option>
                            <option value={'Past Six Months'}>Past Six Months</option>
                            <option value={'Lifetime'}>Lifetime</option>
                    </select>
                </div>
            </div>
            {/* <span>
                {selectedSensorInfo.sensorID}
                {selectedSensorInfo.sensorName}
            </span> */}
            {(Object.keys(selectedSensorInfo).length !== 0) ? 
                <VisualizationBox 
                    sensor={selectedSensorInfo}
                    sensorData={sensorData}
                /> : 
                null}
        </div>
    )
}
export default DataSelectors;