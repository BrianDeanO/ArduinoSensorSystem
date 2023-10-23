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
import { monthsOfTheYear as labels } from "../Helpers/monthsOfTheYear";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export const options = {
  responsive: true,
  plugins: {
    legend: {
      position: 'bottom',
    },
    title: {
      display: true,
      text: 'Stonks',
    }
  },
  scales: {
    y: {
      min: 0,
      max: 20,
      ticks: {
        // Include a dollar sign in the ticks
        callback: function(value, index, ticks) {
          return '$' + value + ' mil';
      }
      }
    }
  }
};

export const data = {
  labels,
  datasets: [
    {
      label: 'Hecate Project',
      data: [5, 12, 8, 5, 5, 11, 14, 8, 11, 5, 6, 12],
      // Could do  different colors based on sensor type
      borderColor: 'rgb(0, 128, 0)',
      backgroundColor: 'rgba(0, 128, 0, 0.856)',
    },
  //   {
  //     label: 'Dataset 2',
  //     data: [0, 2, 8, 16, 8, 4, 15, 11, 10, 7, 4, 15],
  //     borderColor: 'rgb(53, 162, 235)',
  //     backgroundColor: 'rgba(53, 162, 235, 0.5)',
  //   },
  ],
};

const GraphContainer = () => {
    return (
        <div className="mainGraphBox">
            <Line options={options} data={data}/>
        </div>
    )
}

export default GraphContainer;