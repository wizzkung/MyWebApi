import React, { useState } from 'react';
import { Button, Input, Row, Col, Select } from 'antd';

const { Option } = Select;

/*
  Модели (DTO) на фронте (условные):
    - About:        { id, phoneNum, old_pass, new_pass }
    - Stuff:        { first_name, last_name, dt, phoneNum, status, login, password, userType, specializations }
    - Clients:      { first_name, last_name, dt, phoneNum, gender, login, password }
*/

function App() {
    // ----------- GET SCHEDULE -----------
    const [schedule, setSchedule] = useState([]);

    const getSchedule = async () => {
        try {
            const res = await fetch('http://localhost:36449/api/My/GetSchedule');
            if (!res.ok) {
                throw new Error(await res.text() || 'Ошибка при получении расписания');
            }
            const data = await res.json();
            setSchedule(data);
        } catch (error) {
            console.error('Ошибка getSchedule:', error);
        }
    };

    // ----------- CHANGE ABOUT -----------
    const [aboutId, setAboutId] = useState('');
    const [phoneNum, setPhoneNum] = useState('');
    const [oldPass, setOldPass] = useState('');
    const [newPass, setNewPass] = useState('');

    const changeAbout = async () => {
        try {
            const body = {
                id: aboutId,
                phoneNum: phoneNum,
                old_pass: oldPass,
                new_pass: newPass
            };
            const res = await fetch('http://localhost:36449/api/My/ChangeAbout', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
            if (!res.ok) {
                throw new Error(await res.text() || 'Ошибка ChangeAbout');
            }
            alert('Данные успешно обновлены');
        } catch (error) {
            console.error('Ошибка changeAbout:', error);
        }
    };

    // ----------- ADD STUFF -----------
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [dt, setDt] = useState('');
    const [stuffPhone, setStuffPhone] = useState('');
    const [status, setStatus] = useState('');
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [userType, setUserType] = useState('');
    const [specializations, setSpecializations] = useState([]); // multiple

    const addStuff = async () => {
        try {
            const body = {
                first_name: firstName,
                last_name: lastName,
                dt: dt,
                phoneNum: stuffPhone,
                status: status,
                login: login,
                password: password,
                userType: userType,
                specializations: specializations // массив
            };
            const res = await fetch('http://localhost:36449/api/My/AddStuff', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body),
            });
            if (!res.ok) {
                throw new Error(await res.text() || 'Ошибка AddStuff');
            }
            alert('Сотрудник успешно добавлен');
        } catch (error) {
            console.error('Ошибка addStuff:', error);
        }
    };

    // ----------- ADD CLIENT -----------
    const [cFirstName, setCFirstName] = useState('');
    const [cLastName, setCLastName] = useState('');
    const [cDt, setCDt] = useState('');
    const [cPhone, setCPhone] = useState('');
    const [cGender, setCGender] = useState('');
    const [cLogin, setCLogin] = useState('');
    const [cPass, setCPass] = useState('');

    const addClient = async () => {
        try {
            const body = {
                first_name: cFirstName,
                last_name: cLastName,
                dt: cDt,
                phoneNum: cPhone,
                gender: cGender,
                login: cLogin,
                password: cPass
            };
            const res = await fetch('http://localhost:36449/api/My/AddClient', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body),
            });
            if (!res.ok) {
                throw new Error(await res.text() || 'Ошибка AddClient');
            }
            alert('Клиент успешно добавлен');
        } catch (error) {
            console.error('Ошибка addClient:', error);
        }
    };

    // =========== РЕНДЕР ===========
    return (
        <div style={{ padding: '20px' }}>
            <Row gutter={[16, 16]}>
                {/* GET SCHEDULE */}
                <Col span={24}>
                    <h2>Получить расписание</h2>
                    <Button onClick={getSchedule} type="primary">
                        Get Schedule
                    </Button>
                    <div style={{ marginTop: 16 }}>
                        {schedule.map((item, index) => (
                            <Row key={index} style={{ border: '1px solid #ccc', marginBottom: 4 }}>
                                {/* Предположим, что в расписании есть поля id, day, time, etc. */}
                                <Col span={6}>ID: {item.id}</Col>
                                <Col span={6}>Day: {item.day}</Col>
                                <Col span={6}>Time: {item.time}</Col>
                            </Row>
                        ))}
                    </div>
                </Col>

                {/* CHANGE ABOUT */}
                <Col span={24}>
                    <h2>Смена пароля / телефона</h2>
                    <Row gutter={[8, 8]}>
                        <Col>
                            <Input
                                placeholder="ID клиента"
                                value={aboutId}
                                onChange={e => setAboutId(e.target.value)}
                            />
                        </Col>
                        <Col>
                            <Input
                                placeholder="Номер телефона"
                                value={phoneNum}
                                onChange={e => setPhoneNum(e.target.value)}
                            />
                        </Col>
                        <Col>
                            <Input
                                placeholder="Старый пароль"
                                value={oldPass}
                                onChange={e => setOldPass(e.target.value)}
                            />
                        </Col>
                        <Col>
                            <Input
                                placeholder="Новый пароль"
                                value={newPass}
                                onChange={e => setNewPass(e.target.value)}
                            />
                        </Col>
                        <Col>
                            <Button onClick={changeAbout} type="primary">
                                Change About
                            </Button>
                        </Col>
                    </Row>
                </Col>

                {/* ADD STUFF */}
                <Col span={24}>
                    <h2>Добавить Сотрудника</h2>
                    <Row gutter={[8, 8]}>
                        <Col><Input placeholder="Имя" value={firstName} onChange={e => setFirstName(e.target.value)} /></Col>
                        <Col><Input placeholder="Фамилия" value={lastName} onChange={e => setLastName(e.target.value)} /></Col>
                        <Col><Input placeholder="Дата рождения" value={dt} onChange={e => setDt(e.target.value)} /></Col>
                        <Col><Input placeholder="Телефон" value={stuffPhone} onChange={e => setStuffPhone(e.target.value)} /></Col>
                        <Col>
                            <Select
                                placeholder="Статус"
                                style={{ width: 120 }}
                                value={status}
                                onChange={value => setStatus(value)}
                            >
                                <Option value="active">Active</Option>
                                <Option value="fired">Fired</Option>
                            </Select>
                        </Col>
                        <Col><Input placeholder="Login" value={login} onChange={e => setLogin(e.target.value)} /></Col>
                        <Col><Input placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} /></Col>
                        <Col>
                            <Select
                                placeholder="User Type"
                                style={{ width: 120 }}
                                value={userType}
                                onChange={value => setUserType(value)}
                            >
                                <Option value="admin">Admin</Option>
                                <Option value="worker">Worker</Option>
                            </Select>
                        </Col>
                        <Col>
                            <Select
                                mode="multiple"
                                placeholder="Specializations"
                                style={{ width: 180 }}
                                value={specializations}
                                onChange={value => setSpecializations(value)}
                            >
                                <Option value="Yoga">Yoga</Option>
                                <Option value="Fitness">Fitness</Option>
                                <Option value="Boxing">Boxing</Option>
                            </Select>
                        </Col>
                        <Col>
                            <Button onClick={addStuff} type="primary">Add Stuff</Button>
                        </Col>
                    </Row>
                </Col>

                {/* ADD CLIENT */}
                <Col span={24}>
                    <h2>Добавить Клиента</h2>
                    <Row gutter={[8, 8]}>
                        <Col><Input placeholder="Имя" value={cFirstName} onChange={e => setCFirstName(e.target.value)} /></Col>
                        <Col><Input placeholder="Фамилия" value={cLastName} onChange={e => setCLastName(e.target.value)} /></Col>
                        <Col><Input placeholder="Дата рождения" value={cDt} onChange={e => setCDt(e.target.value)} /></Col>
                        <Col><Input placeholder="Телефон" value={cPhone} onChange={e => setCPhone(e.target.value)} /></Col>
                        <Col>
                            <Select
                                placeholder="Пол"
                                style={{ width: 120 }}
                                value={cGender}
                                onChange={value => setCGender(value)}
                            >
                                <Option value="male">Male</Option>
                                <Option value="female">Female</Option>
                            </Select>
                        </Col>
                        <Col><Input placeholder="Login" value={cLogin} onChange={e => setCLogin(e.target.value)} /></Col>
                        <Col><Input placeholder="Password" value={cPass} onChange={e => setCPass(e.target.value)} /></Col>
                        <Col>
                            <Button onClick={addClient} type="primary">Add Client</Button>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </div>
    );
}

export default App;
