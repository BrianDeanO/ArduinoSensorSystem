import React, { useEffect, useState, Component } from "react";
//@ts-ignore
import Selectors from "./DataAndTimeSelectors/selectors.js";
//@ts-ignore
import TemperatureVisualizationBox from "./Visualizations/TemperatureVisualizationBox.js";

// import { variables, dataObject } from "../variables.js";


const HecateBody = () => {
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
            <div>
            </div>
        </div>  
    )
}

export default HecateBody;