$(document).ready(function () {
    $("#getScheduleBtn").click(function () {
        $.ajax({
            url: "http://localhost:5272/api/My/GetSchedule",
            method: "GET",
            success: function (data) {
                $("#scheduleData").empty();
                console.log("Получено расписание:", data);


                var html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr><th>Групповые</th><th>Индивидуальные</th><th>Тренер</th></tr></thead><tbody>';
                data.forEach(function (item) {
                    html += `<tr>
                <td>${item.group_schedule}</td>
                <td>${item.individual_schedule}</td>
                <td>${item.last_name}</td>
              </tr>`;
                });

                html += '</tbody></table>';
                $("#scheduleData").html(html);
            },
            error: function (err) {
                alert('Ошибка при получении расписания');
                console.log(err);
            }
        });
    });


    $("#changeAboutBtn").click(function () {
        var about = {
            id: $("#aboutId").val(),
            phoneNum: $("#phoneNum").val(),
            old_pass: $("#oldPass").val(),
            new_pass: $("#newPass").val()
        };

        $.ajax({
            url: "http://localhost:5272/api/My/ChangeAbout",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(about),
            success: function (response) {
                alert('Данные успешно обновлены');
                console.log("ChangeAbout:", response);
            },
            error: function (err) {
                alert('Ошибка: ' + err.responseText);
                console.log("ChangeAbout error:", err);
            }
        });
    });


    $("#addStuffBtn").click(function () {
        var stuff = {
            first_name: $("#firstName").val(),
            last_name: $("#lastName").val(),
            dt: $("#dt").val(),
            phoneNum: $("#stuffPhone").val(),
            status: $("#status").val(),
            login: $("#login").val(),
            password: $("#password").val(),
            userType: $("#userType").val(),
            specializations: $("#specializations").val().split(',')
        };

        $.ajax({
            url: "http://localhost:5272/api/My/AddStuff",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(stuff),
            success: function (response) {
                alert('Сотрудник успешно добавлен');
                console.log("AddStuff:", response);
            },
            error: function (err) {
                alert('Ошибка: ' + err.responseText);
                console.log("AddStuff error:", err);
            }
        });
    });

    $("#addClientBtn").click(function () {
        var client = {
            first_name: $("#cFirstName").val(),
            last_name: $("#cLastName").val(),
            dt: $("#cDt").val(),
            phoneNum: $("#cPhone").val(),
            gender: $("#cGender").val(),
            login: $("#cLogin").val(),
            password: $("#cPass").val()
        };

        $.ajax({
            url: "http://localhost:5272/api/My/AddClient",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(client),
            success: function (response) {
                alert('Клиент успешно добавлен');
                console.log("AddClient:", response);
            },
            error: function (err) {
                alert('Ошибка: ' + err.responseText);
                console.log("AddClient error:", err);
            }
        });
    });

    $("#addScheduleBtn").click(function () {
        var schedule = {
            stuff_id: $("#cStuff_id").val(),
            specialization_id: $("#сSpecialization_id").val(),
            group_schedule: $("#cGroupSchedule").val(),
            individual_schedule: $("#cIndividual_schedule").val()
        };

        $.ajax({
            url: "http://localhost:5272/api/My/AddSchedule",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(schedule),
            success: function (response) {
                alert('Клиент успешно добавлен');
                console.log("AddSchedule:", response);
            },
            error: function (err) {
                alert('Ошибка: ' + err.responseText);
                console.log("AddSchedule error:", err);
            }
        });
    });

    $('#Auth').click(function () {
        $('#loginModal').modal('show');
    });


    $('#loginButton').click(function () {
        const data = {
            login: $('#loginInput').val(),
            password: $('#passwordInput').val()
        };

        $.ajax({
            url: 'http://localhost:5272/api/Token/GetToken',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (resp) {
                if (resp.status === 'OK') {
                    localStorage.setItem('jwtToken', resp.token);
                    setupAjaxToken(resp.token);
                    const decoded = decodeJWT(resp.token);
                    console.log('JWT claims:', decoded);
                    window.currentUserType = decoded.userType;
                    $('#mainContent').show();
                    $('#Auth').hide();
                    if (decoded.userType === 'Admin') {
                        $('#admin').show();
                    } else {
                        $('#adminStuff').hide();
                        $('#adminClient').hide();
                        $('#getClientsBtn').hide();
                        $('#adminSchedule').hide();
                        $('#h2Schedule').hide();
                        $('#h2Client').hide();
                        $('#h2Stuff').hide();






                    }

                    alert('Авторизация успешна!');
                } else {
                    alert('Ошибка авторизации');
                }
            },
            error: function (err) {
                console.error('Ошибка получения токена', err);
                alert('Ошибка логина: ' + err.responseText);
            }
        });
    });

    $('#getScheduleBtn').click(function () {
        $.ajax({
            url: 'http://localhost:5272/api/My/GetSchedule',
            method: 'GET',
            success: function (data) {
                $('#scheduleData').empty();
                console.log("Получено расписание:", data);

                let html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr><th>Групповые</th><th>Индивидуальные</th><th>Тренер</th></tr></thead><tbody>';
                data.forEach(item => {
                    html += `<tr>
            <td>${item.group_schedule}</td>
            <td>${item.individual_schedule}</td>
            <td>${item.last_name}</td>
          </tr>`;
                });
                html += '</tbody></table>';

                $('#scheduleData').html(html);
            },
            error: function (err) {
                alert('Ошибка при получении расписания');
                console.log(err);
            }
        });
    });
    $('#getStuffBtn').click(function () {
        getStuff();
    });

    $('#getClientsBtn').click(function () {
        getClients();
    });
    $('#getGymsBtn').click(function () {
        getGyms();
    });


    function getStuff() {
        $.ajax({
            url: 'http://localhost:5272/api/My/GetStuff',
            method: 'GET',
            success: function (data) {
                $('#listData').empty();
                console.log("Сотрудники:", data);
                let html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr><th>ID</th><th>Имя</th><th>Фамилия</th><th>Телефон</th><th>Статус</th>';
                if (window.currentUserType === 'Admin') {
                    html += '<th>Удалить</th>';
                }

                html += '</tr></thead><tbody>';

                data.forEach(item => {
                    html += `<tr>
          <td>${item.id}</td>
          <td>${item.first_name}</td>
          <td>${item.last_name}</td>
          <td>${item.phoneNum}</td>
          <td>${item.status}</td>`;
                    if (window.currentUserType === 'Admin') {
                        html += `<td>
            <button class="deleteStuffBtn btn btn-danger btn-sm" data-id="${item.id}">X</button>
          </td>`;
                    }

                    html += '</tr>';
                });

                html += '</tbody></table>';
                $('#listData').html(html);
            },
            error: function (err) {
                alert('Ошибка при получении списка сотрудников');
                console.log(err);
            }
        });
    }


    function getClients() {
        $.ajax({
            url: 'http://localhost:5272/api/My/GetClients',
            method: 'GET',
            success: function (data) {
                $('#listData').empty();
                console.log("Клиенты:", data);

                let html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr><th>ID</th><th>Имя</th><th>Фамилия</th><th>Пол</th><th>Дата Рожд.</th><th></th></tr></thead><tbody>';
                data.forEach(item => {
                    html += `<tr>
            <td>${item.id}</td>
            <td>${item.first_name}</td>
            <td>${item.last_name}</td>
            <td>${item.gender}</td>
            <td>${item.dateBirth}</td>
            <td>
              <button class="deleteClientBtn btn btn-danger btn-sm" data-id="${item.id}">X</button>
            </td>
          </tr>`;
                });
                html += '</tbody></table>';

                $('#listData').html(html);
            },
            error: function (err) {
                alert('Ошибка при получении списка клиентов');
                console.log(err);
            }
        });
    }

    function getGyms() {
        $.ajax({
            url: 'http://localhost:5272/api/My/ShowGyms',
            method: 'GET',
            success: function (data) {
                $('#listData').empty();
                console.log("Список залов:", data);

                let html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr><th>Название</th><th>Адрес</th><th>Статус</th>';
                if (window.currentUserType === 'Admin') {
                    html += '<th>Действие</th>';
                }

                html += '</tr></thead><tbody>';

                data.forEach(item => {
                    let circle = item.Active
                        ? '<span style="color: green;">●</span>'
                        : '<span style="color: red;">●</span>';

                    html += `<tr>
          <td>${item.name}</td>
          <td>${item.address}</td>
          <td>${circle}</td>`;
                    if (window.currentUserType === 'Admin') {
                        html += `<td>
            <button class="toggleActivityBtn btn btn-warning btn-sm" data-id="${item.id}">
              Изменить
            </button>
          </td>`;
                    }

                    html += '</tr>';
                });

                html += '</tbody></table>';

                $('#listData').html(html);
            },
            error: function (err) {
                alert('Ошибка при получении списка залов');
                console.log(err);
            }
        });
    }


    $(document).on('click', '.deleteStuffBtn', function () {
        const stuffId = $(this).data('id');
        if (!confirm(`Удалить сотрудника #${stuffId}?`)) {
            return;
        }

        $.ajax({
            url: `http://localhost:5272/api/My/DeleteStuff/${stuffId}`,
            method: 'DELETE',
            success: function (resp) {
                alert(resp);

                getStuff();
            },
            error: function (err) {
                alert('Ошибка при удалении сотрудника');
                console.log(err);
            }
        });
    });


    $(document).on('click', '.toggleActivityBtn', function () {
        const gymId = $(this).data('id');
        $.ajax({
            url: `http://localhost:5272/api/My/ChangeGymStatus/${gymId}`,
            method: 'GET',
            success: function (resp) {
                alert(resp.message || 'Статус изменен');
                getGyms();
            },
            error: function (err) {
                alert('Ошибка при смене статуса активности');
                console.log(err);
            }
        });
    });





    $(document).on('click', '.deleteClientBtn', function () {
        const clientId = $(this).data('id');
        if (!confirm(`Удалить клиента #${clientId}?`)) {
            return;
        }
        $.ajax({
            url: `http://localhost:5272/api/My/DeleteClient/${clientId}`,
            method: 'DELETE',
            success: function (resp) {
                alert(resp);
                getClients();
            },
            error: function (err) {
                alert('Ошибка при удалении клиента');
                console.log(err);
            }
        });
    });




    function decodeJWT(token) {
        const payload = token.split('.')[1];
        const decodedPayload = atob(payload);
        return JSON.parse(decodedPayload);
    }
    function setupAjaxToken(token) {
        $.ajaxSetup({
            headers: {
                'Authorization': 'Bearer ' + token
            }
        });
    }





});