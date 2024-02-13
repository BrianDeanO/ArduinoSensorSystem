import React, { useCallback, useEffect, useState } from "react";
import { SensorType, SensorDTOType, DeviceType } from "../interfaces";
import { channelCountArray, proxyURL } from "../Variables";
import axios from "axios";

interface ConfigureDeviceProps {
    configureDevice: (configuringDevice: boolean) => void;
    selectedDeviceID: number;
}

const ConfigureDevice: React.FC<ConfigureDeviceProps> = (
    {
    configureDevice,
    selectedDeviceID
}: ConfigureDeviceProps
) => {  
    return (
        <div className="ConfigDeviceMainBox">
            configuring device ID - {selectedDeviceID}
            <div className="DeviceSubmitAndCancelMainBox">
                    <button 
                        className="mainButton"
                        onClick={(e) => {

                        }}>
                            Update Device
                    </button>
                    <button 
                        className="mainButton"
                        onClick={(e) => {
                            configureDevice(false)
                        }}>
                            Cancel
                    </button>
                </div>
        </div>
    )
}

export default ConfigureDevice;