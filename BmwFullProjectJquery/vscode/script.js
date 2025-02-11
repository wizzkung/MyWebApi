$(document).ready(function () {
    // Load initial data with animation
    loadBMWModels();

    // Search functionality with animation
    $('#searchInput').on('keyup', function () {
        const searchTerm = $(this).val().toLowerCase();
        $('#bmwTable tbody tr').each(function () {
            const $row = $(this);
            const matches = $row.text().toLowerCase().indexOf(searchTerm) > -1;

            if (matches) {
                $row.fadeIn(300);
            } else {
                $row.fadeOut(300);
            }
        });
    });

    // Add new model button animation
    $('#addNew').hover(
        function () { $(this).addClass('animate__animated animate__pulse'); },
        function () { $(this).removeClass('animate__animated animate__pulse'); }
    );

    // Add new model button
    $('#addNew').click(function () {
        $('#modalTitle').text('Add New BMW Model');
        $('#modalAction').val('add');
        $('#modelForm')[0].reset();
        $('#oldModelName').val('');
        $('#modelModal').fadeIn(300);
        $('.modal-content').addClass('animate__animated animate__slideInDown');
    });


    // Show All button
    $('#showAll').click(function () {
        loadBMWModels();
    });

    // Show by Series button
    $('#showBySeries').click(function () {
        const series = $('#searchInput').val();
        if (series) {
            filterBySeries(series);
        } else {
            alert('Please enter a series in the search box');
        }
    });

    // Show by Model button
    $('#showByModel').click(function () {
        const model = $('#searchInput').val();
        if (model) {
            filterByModel(model);
        } else {
            alert('Please enter a model in the search box');
        }
    });

    // Close modal with animation
    $('.close').click(function () {
        $('.modal-content').addClass('animate__animated animate__fadeOutUp');
        setTimeout(() => {
            $('#modelModal').fadeOut(300);
            $('.modal-content').removeClass('animate__animated animate__fadeOutUp');
        }, 300);
    });

    // Form submit handler with animation
    $('#modelForm').submit(function (e) {
        e.preventDefault();
        const action = $('#modalAction').val();
        if (action === 'add') {
            createModel();
        } else {
            updateModel();
        }

        $('.modal-content').addClass('animate__animated animate__fadeOutUp');
        setTimeout(() => {
            $('#modelModal').fadeOut(300);
            $('.modal-content').removeClass('animate__animated animate__fadeOutUp');
        }, 300);
    });

    // Export buttons animation
    $('.btn-export').hover(
        function () { $(this).addClass('animate__animated animate__pulse'); },
        function () { $(this).removeClass('animate__animated animate__pulse'); }
    );
});

function loadBMWModels() {
    $.ajax({
        url: 'http://localhost:5099/api/Rest/GetAll',
        method: 'GET',
        success: function (data) {
            renderTable(data);
            // Animate table rows on load
            $('#bmwTable tbody tr').each(function (index) {
                $(this).hide().delay(index * 100).fadeIn(500);
            });
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                alert('Unauthorized. Please log in again.');
                // Можно добавить здесь логику для перенаправления на страницу входа
            } else {
                alert('Error loading models: ' + xhr.responseText);
            }
        }
    });
}

function renderTable(data) {
    const tbody = $('#bmwTable tbody');
    tbody.empty();

    data.forEach(model => {
        const row = $(`
            <tr data-model="${model.name}" data-series="${model.series}">
                <td>${model.name}</td>
                <td>${model.series}</td>
                <td class="action-cell">
                    <button class="btn-secondary btn-edit" onclick="editModel('${model.name}', '${model.series}')">Edit</button>
                    <button class="btn-secondary btn-delete" onclick="deleteModel('${model.name}', '${model.series}')">Delete</button>
                    <button class="btn-secondary btn-about" onclick="showAbout('${model.name}')">About</button>
                </td>
            </tr>
        `).hide();

        tbody.append(row);
        row.fadeIn(500);
    });
}

function createModel() {
    const modelData = {
        name: $('#modelName').val(),
        seriesName: $('#series').val()
    };
    $.ajax({
        url: 'http://localhost:5099/api/Rest/AddModel',
        method: 'POST',
        data: JSON.stringify(modelData),
        contentType: 'application/json',
        success: function (response) {
            alert(response); // Show the success message
            loadBMWModels();
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                alert('Unauthorized. Please log in again.');
            } else {
                alert('Error adding model: ' + xhr.responseText);
            }
        }
    });
}

function editModel(oldName, seriesName) {
    $('#modalTitle').text('Edit BMW Model');
    $('#modalAction').val('edit');
    $('#oldModelName').val(oldName);
    $('#modelName').val(oldName);
    $('#series').val(seriesName);
    $('#modelModal').fadeIn(300);
    $('.modal-content').addClass('animate__animated animate__slideInDown');
}

function updateModel() {
    const oldName = $('#oldModelName').val();
    const newName = $('#modelName').val();
    const seriesName = $('#series').val();

    $.ajax({
        url: 'http://localhost:5099/api/Rest/Update',
        method: 'POST',
        data: JSON.stringify({
            oldName: oldName,
            dto: {
                name: newName,
                seriesName: seriesName
            }
        }),
        contentType: 'application/json',
        success: function (response) {
            alert(response); // Show the success message
            $('#modelModal').fadeOut(300);
            loadBMWModels();
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                alert('Unauthorized. Please log in again.');
            } else {
                alert('Error updating model: ' + xhr.responseText);
            }

        }
    });
}

function deleteModel(modelName, seriesName) {
    if (confirm('Are you sure you want to delete this model?')) {
        const row = $(`#bmwTable tr[data-model="${modelName}"][data-series="${seriesName}"]`);
        row.addClass('row-delete');

        setTimeout(() => {
            $.ajax({
                url: `http://localhost:5099/api/Rest/Delete/${encodeURIComponent(modelName)}/${encodeURIComponent(seriesName)}`,
                method: 'DELETE',
                success: function (response) {
                    alert(response); // Show the success message
                    loadBMWModels();
                },
                error: function (xhr, status, error) {
                    if (xhr.status === 401) {
                        alert('Unauthorized. Please log in again.');
                    } else {
                        alert('Error deleting model: ' + xhr.responseText);
                    }
                }
            });
        }, 500);
    }
}
$('#exportExcel').click(function () {
    exportExcel();
});

$('#exportCSV').click(function () {
    exportCsv();
});

function exportExcel() {
    $.ajax({
        url: 'http://localhost:5099/api/Rest/excel',
        method: 'GET',
        // указываем, что ждем blob (файл)
        xhrFields: { responseType: 'blob' },
        success: function (blob) {
            // Создаём ссылку на blob и автоматически кликаем её
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = 'Report.xlsx'; // Имя файла
            document.body.appendChild(a);
            a.click();

            // Очищаем ссылки, чтобы не засорять память
            a.remove();
            URL.revokeObjectURL(downloadUrl);
        },
        error: function (xhr) {
            alert('Ошибка при выгрузке Excel: ' + xhr.responseText);
        }
    });
}

function exportCsv() {
    $.ajax({
        url: 'http://localhost:5099/api/Rest/csv', // Подставьте ваш реальный эндпоинт
        method: 'GET',
        xhrFields: { responseType: 'blob' },
        success: function (blob) {
            // Создаём ссылку на blob
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = 'Report.csv';
            document.body.appendChild(a);
            a.click();

            // Удаляем ссылку, освобождаем память
            a.remove();
            URL.revokeObjectURL(downloadUrl);
        },
        error: function (xhr) {
            alert('Ошибка при выгрузке CSV: ' + xhr.responseText);
        }
    });
}

function showAbout(modelName) {
    // Показываем загрузчик
    $('#aboutModalContent').html(`
        <div class="loading-spinner">
            <div class="spinner"></div>
            <p>Generating AI-powered description for ${modelName}...</p>
        </div>
    `);

    $('#aboutModal').fadeIn(300);
    $('.modal-about-content').addClass('animate__animated animate__slideInDown');

    // Делаем запрос к API
    $.ajax({
        url: 'http://localhost:5099/api/Rest/deepSeek',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ ModelName: modelName }),
        success: function (response) {
            const content = `
                <h3>BMW ${modelName}</h3>
                <div class="ai-description">${marked.parse(response.description)}</div>
                <div class="disclaimer">
                    <small>*AI-generated content. Verify with official sources.</small>
                </div>
            `;
            $('#aboutModalContent').html(content);
        },
        error: function (xhr) {
            $('#aboutModalContent').html(`
                <div class="error-alert">
                    <p>⚠️ Error generating description</p>
                    <p>${xhr.responseJSON?.error || 'Please try again later'}</p>
                </div>
            `);
        }
    });
}


// Sign In button
$('#signIn').click(function () {
    $('#signInModal').fadeIn(300);
    $('.modal-sign-in-content').addClass('animate__animated animate__slideInDown');
});

// Close sign in modal with animation
$('.close-sign-in').click(function () {
    $('.modal-sign-in-content').addClass('animate__animated animate__fadeOutUp');
    setTimeout(() => {
        $('#signInModal').fadeOut(300);
        $('.modal-sign-in-content').removeClass('animate__animated animate__fadeOutUp');
    }, 300);
});

// Sign In form submit handler
$('#signInForm').submit(function (e) {
    e.preventDefault();
    const username = $('#username').val();
    const password = $('#password').val();

    // Send login request to backend
    $.ajax({
        url: 'http://localhost:5099/api/JWT/GetToken',
        method: 'POST',
        data: JSON.stringify({ login: username, password: password }),
        contentType: 'application/json',
        success: function (response) {
            // Предполагаем, что ответ - это непосредственно токен
            localStorage.setItem('jwtToken', response.result);
            alert('Successfully logged in!');
            $('#signInModal').fadeOut(300);
            // Здесь можно обновить UI, чтобы показать, что пользователь вошел в систему
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                alert('Invalid username or password');
            } else {
                alert('Login failed: ' + xhr.responseText);
            }
        }
    });
});

// Close modals when clicking outside
$(window).click(function (event) {
    if ($(event.target).is('#modelModal') || $(event.target).is('#aboutModal') || $(event.target).is('#signInModal')) {
        const modalContent = $(event.target).find('.modal-content, .modal-about-content, .modal-sign-in-content');
        modalContent.addClass('animate__animated animate__fadeOutUp');
        setTimeout(() => {
            $(event.target).fadeOut(300);
            modalContent.removeClass('animate__animated animate__fadeOutUp');
        }, 300);
    }
});


function filterBySeries(series) {
    $.ajax({
        url: `http://localhost:5099/api/Rest/GetBySeries/=${series}`,
        method: 'GET',
        success: function (data) {
            renderTable(data);
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                alert('Unauthorized. Please log in again.');
            } else {
                alert('Error filtering by series: ' + xhr.responseText);
            }
        }
    });
}

function filterByModel(model) {
    $.ajax({
        url: `http://localhost:5099/api/Rest/GetByModel/=${model}`,
        method: 'GET',
        success: function (data) {
            renderTable(data);
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                alert('Unauthorized. Please log in again.');
            } else {
                alert('Error filtering by model: ' + xhr.responseText);
            }
        }
    });
}


// Close modal when clicking outside
$(window).click(function (event) {
    if ($(event.target).is('#modelModal')) {
        $('.modal-content').addClass('animate__animated animate__fadeOutUp');
        setTimeout(() => {
            $('#modelModal').fadeOut(300);
            $('.modal-content').removeClass('animate__animated animate__fadeOutUp');
        }, 300);
    }
});

$('.btn-export').hover(
    function () { $(this).addClass('animate__animated animate__pulse'); },
    function () { $(this).removeClass('animate__animated animate__pulse'); }
);

// Close about modal with animation
$('.close-about').click(function () {
    $('.modal-about-content').addClass('animate__animated animate__fadeOutUp');
    setTimeout(() => {
        $('#aboutModal').fadeOut(300);
        $('.modal-about-content').removeClass('animate__animated animate__fadeOutUp');
    }, 300);
});

// Close about modal when clicking outside
$(window).click(function (event) {
    if ($(event.target).is('#aboutModal')) {
        $('.modal-about-content').addClass('animate__animated animate__fadeOutUp');
        setTimeout(() => {
            $('#aboutModal').fadeOut(300);
            $('.modal-about-content').removeClass('animate__animated animate__fadeOutUp');
        }, 300);
    }
});

$.ajaxSetup({
    beforeSend: function (xhr) {
        const token = localStorage.getItem('jwtToken');
        if (token) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        }
    }
});