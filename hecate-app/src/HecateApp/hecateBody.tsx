import React, { useEffect, useState, Component } from "react";
import Selectors from "./DataAndTimeSelectors/selectors.js";
import TemperatureVisualizationBox from "./Visualizations/TemperatureVisualizationBox.js";

// import { variables, dataObject } from "../variables.js";

import Devices from "./Devices.tsx";

interface BodyProps {
    loggedIn: boolean;
}

const HecateBody: React.FC<BodyProps>  = ({loggedIn}: BodyProps) => {
    // const [tData, setTData] = useState([]);

    // function refreshList() {
    //     fetch(variables.API_URL)
    //     .then(response => response.json())
    //     .then(data => {
    //         setTData(data)
    //     });
    // }

    // useEffect(() => {
    //     refreshList();
    // }, [tData]);
    
    return (
        <div className="HecateBody">
            {/* <Selectors /> */}
            {/* <TemperatureVisualizationBox /> */}
            <Devices loggedIn={loggedIn}/>
        </div>  
    )
}

export default HecateBody;