import { Button, Input, Select } from 'antd';
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
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' }
    };
    fetch(`http://localhost:36449/api/My/GetAll`, requestOptions)
      .then(response => {
        return response.json()
      })
      .then(data => {
        console.log(data)
        setDataSelect(data);
      })
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


    </div>
  )
}

export default Home;
