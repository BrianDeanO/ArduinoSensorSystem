import React, { useState } from "react";

const SensorSelector = (sensorList) => {
    const [selectorType, setSelectorType] = useState('');

    // Need to put selectorType in local storage.

    return (
        <div className="sensorTypeSelectorBox">
            <div className="sensorTypeSelectorText">Sensor Type</div>
            <select 
                className="sensorTypeSelector" 
                id="sensorType"
                onClick={(e) => {
                    //@ts-ignore
                    setSelectorType(e.target.value);
                }}>
                    {
                        sensorList.map((sensor) => {
                            return (
                                <option value={sensor.sensor_type}>
                                    {sensor.sensor_type}
                                </option>
                            )
                            
                        })
                    }
                    {/* <option value={'---'}>---</option>
                    <option value={'Temperature'}>Temperature</option>
                    <option value={'Irradiance'}>Irradiance</option> */}
            </select>
        </div>
    )
}

export default SensorSelector;