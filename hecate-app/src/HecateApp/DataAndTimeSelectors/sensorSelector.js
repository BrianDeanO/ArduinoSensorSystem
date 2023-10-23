import React, { useState } from "react";

const SensorSelector = () => {
    const [selectorType, setSelectorType] = useState("");
    // const [selectorType, setSelectorType] = useState(document.getElementById('sensorType')?.value ? document.getElementById('sensorType')?.value : '');

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
                    <option value={'---'}>---</option>
                    <option value={'Temperature'}>Temperature</option>
                    <option value={'Irridiance'}>Irridiance</option>
            </select>
        </div>
    )
}

export default SensorSelector;