import React from "react";
import SensorSelector from "./sensorSelector.js";
import TimeSelector from "./timeSelector.js";

const Selectors = () => {
    return (
        <div className="mainSelectorBox">
            <SensorSelector />
            <TimeSelector />
        </div>
    )
}

export default Selectors;