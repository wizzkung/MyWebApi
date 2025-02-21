import { Button, Input, Select } from 'antd';
import { data, error } from 'jquery';
import React, { Component, useState, useEffect } from 'react';

const { Option } = Select;

const Home = () => {

  const [loginAdd, setLoginAdd] = useState('')

  const [mySelect, setMySelect] = useState('');
  const handleChangeMySelect = (value) => {
    console.log(value)
    setMySelect(value);
  };

  const [dataSelect, setDataSelect] = useState([]);
  const FillSelect = () => {
    fetch(`http://localhost:36449/api/My/GetAll`)
      .then(response => response.json())
      .then(data => {
        console.log('Все модели после добавления:', data);
        setDataSelect(data);
      })
      .catch(error => console.log('Ошибка в FillSelect:', error));
  };


  const [AddModel, setAddModel] = useState('');
  const [AddSeries, setAddSeries] = useState('');
  const Insert = () => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name: AddModel, seriesName: AddSeries })
    };

    fetch('http://localhost:36449/api/My/AddModel', requestOptions)
      .then(response => {
        if (!response.ok) {
          throw new Error('Ошибка запроса');
        }
        return response.text();
      })
      .then(result => {
        console.log(result);
        // После успешной вставки вызываем FillSelect для обновления списка
        FillSelect();
      })
      .catch(error => {
        console.log(error);
      });

  }

  const MyClick = () => {
    // setLoginAdd('wawawawa')
    FillSelect();
  }
  return (
    <div>
      <h1>Hello, world!</h1>
      <Button
        type='primary'
        onClick={MyClick}>
        Кнопка ANTD
      </Button>

      <Button
        type='primary'
        onClick={Insert}>
      </Button>

      <Input
        placeholder='Input Login'
        value={loginAdd}
        onChange={(e) => setLoginAdd(e.target.value)}
        style={{
          width: 252, marginLeft: 137
        }}
      />
      <Select
        style={{
          width: 252, marginLeft: 145
        }}
        showSearch
        status="warning"
        //defaultValue="2"
        value={mySelect}
        optionFilterProp="children"
        onChange={handleChangeMySelect}
        filterOption={(input, option) => option.children.toLowerCase().includes(input.toLowerCase())}
      >
        {dataSelect.map((z) => (
          <Option key={z.series}>{z.name}</Option>
        ))}

      </Select>
      <Input
        placeholder='Input Model'
        value={AddModel}
        onChange={(e) => setAddModel(e.target.value)}
        style={{
          width: 252, marginLeft: 300
        }}
      />
      <Input
        placeholder='Input Series'
        value={AddSeries}
        onChange={(e) => setAddSeries(e.target.value)}
        style={{
          width: 252, marginLeft: 400
        }}
      />


    </div>
  )
}

export default Home;
