import React, { useEffect, useState } from 'react';


const FetchData = () => {

  const [forecast, setforecast] = useState([])
  const fetchData = () => {
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' }
    };
    fetch(`weatherforecast`, requestOptions)
      .then(response => {
        return response.json()
      })
      .then(data => {
        setforecast(data);
      })
    //const data = response.json();
    //setforecast(data);
    // , loading: false });
  }


  const renderForecastsTable = (forecasts) => {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }


  useEffect(() => {

    fetchData();
  }, []);


  return (
    <div>
      <h1 id="tabelLabel" >Weather forecast</h1>
      <p>This component demonstrates fetching data from the server.</p>
      {renderForecastsTable(forecast)}
    </div>
  );




}
export default FetchData;