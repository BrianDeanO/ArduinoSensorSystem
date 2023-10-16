import React, { useState } from "react";

const TimeSelector = () => {
    const [timeType, setTimeType] = useState("");

    return (
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
    )
}

export default TimeSelector;