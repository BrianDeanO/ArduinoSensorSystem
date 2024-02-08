import React, { useEffect, useState, Component } from "react";
import Sensors from "./Selectors.tsx";

interface DeviceProps {
    loggedIn: boolean;
}

const Devices: React.FC<DeviceProps> = (loggedIn) => {  
    console.log("loggedIn IN DEVICES", loggedIn);  
    return (
        <div className="MainDevicesBox">
            <div className="MainDeviceSelectorBox">
                DEVICES
                {loggedIn.loggedIn ? `\nRICHLAND_1` : ''}
            </div>
            <Sensors />
        </div>  
    )
}

export default Devices;