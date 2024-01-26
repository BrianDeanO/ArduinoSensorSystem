import React, { useEffect, useState } from "react";
import GraphContainer, { data } from "./graphContainer";

const TemperatureVisualizationBox = () => {
    const [tData, setTData] = useState([]);
    let minValue;
    let maxValue;
    let dataLabel = '';

    function refreshList() {
        fetch('Temperature')
        .then(response => response.json())
        .then(data => {
            setTData(data)
        }).catch((error) => (console.log('Fetching Error - ', error)));
    }

    // function refreshList() {
    //     fetch('api/todoitems')
    //     .then(response => response.json())
    //     .then(data => {
    //         setTData(data)
    //     });
    // }

    useEffect(() => {
        refreshList();
    }, []);

    console.log('tData', tData);

    const temperatureValues = tData ? tData.map((d, index) => {
        if(index === 0) {
            //@ts-ignore
            dataLabel = d.TemperatureUnit;
            //@ts-ignore
            minValue = d.TemperatureValue;
            //@ts-ignore
            maxValue = d.TemperatureValue;
        }
        //@ts-ignore
        if(minValue >= d.TemperatureValue) {minValue = d.TemperatureValue};
        //@ts-ignore
        if(maxValue <= d.TemperatureValue) {maxValue = d.TemperatureValue};
        //@ts-ignore
        return d.TemperatureValue
    }) : [];

    const temperatureTimes = tData ? tData.map((d) => {
        //@ts-ignore
        return d.TemperatureTime
    }) : [];
    
    console.log('T data', tData);

    return (
        <div className="mainVisualizationBox">
            <GraphContainer 
                values={temperatureValues} 
                times={temperatureTimes} 
                dataLabel={dataLabel} 
                dataTitle={`Temperature (${dataLabel})`} 
                maxValue={maxValue}
                minValue={minValue}
            /> 
        </div>
    )
}

export default TemperatureVisualizationBox;