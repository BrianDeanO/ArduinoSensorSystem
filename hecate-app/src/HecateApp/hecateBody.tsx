import React from "react";
//@ts-ignore
import Selectors from "./DataAndTimeSelectors/selectors.tsx";
//@ts-ignore
import VisualizationBox from "./Visualizations/visualizationBox.tsx";

const HecateBody = () => {
    return (
        <div className="HecateBody">
            <Selectors />
            <VisualizationBox />
        </div>  
    )
}

export default HecateBody;