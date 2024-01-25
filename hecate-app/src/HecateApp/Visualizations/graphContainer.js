import React from "react";
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';
import NoData from "../Helpers/NoDataPopUp.tsx";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export const options = (min, max, dataLabel, dataTitle) => ({
  responsive: true,
  plugins: {
    legend: {
      position: 'bottom',
    },
    title: {
      display: true,
      text: dataTitle,
    }
  },
  scales: {
    y: {
      min: (((min - Math.ceil(min / 4) < 0) && (min > 0)) ? 0 : (min - Math.ceil(min / 4))),
      max: max + Math.ceil(max / 4),
      ticks: {
        callback: function(value, index, ticks) {
          // return value + ` ${dataLabel}`;
          return value;
      }
      }
    }
  }
});

export const data = (values, times) => ({
  labels: times,
  datasets: [
    {
      label: 'Hecate Project',
      data: values,
      // Could do  different colors based on sensor type
      borderColor: 'rgb(0, 128, 0)',
      backgroundColor: 'rgba(0, 128, 0, 0.856)',
    }
  ],
});

const GraphContainer = (values, times, dataLabel, dataTitle, maxValue, minValue) => {
  // console.log('graph recieved data', values.values);
  // console.log('graph recieved times', values.times);
  // console.log('graph recieved dataLabel', values.dataLabel);
  // console.log('graph recieved maxValue', values.maxValue);
  // console.log('graph recieved minValue', values.minValue);

  return (
      <div className="mainGraphBox">
          { 
            values.values ? 
              <Line options = {options(values.minValue, values.maxValue, values.dataLabel, values.dataTitle)} data={data(values.values, values.times)}/> : 
              <NoData />
          }
      </div>
  )
}

export default GraphContainer;