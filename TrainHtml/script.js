document.addEventListener('DOMContentLoaded', function() {
    const loginBtn = document.getElementById('login-btn');
    const logoutBtn = document.getElementById('logout-btn');
    const userInfo = document.getElementById('user-info');
    const usernameSpan = document.getElementById('username');
    const loginModal = document.getElementById('login-form');
    const registerModal = document.getElementById('register-form');
    const closeButtons = document.querySelectorAll('.close');
    const switchToRegister = document.getElementById('switch-to-register');
    const switchToLogin = document.getElementById('switch-to-login');
    const filterForm = document.getElementById('filter-form');
    const scheduleTable = document.getElementById('schedule-table');
    const scheduleTableBody = document.querySelector('#schedule-table tbody');
    const errorMessage = document.getElementById('error-message');
    const phoneInput = document.getElementById('phone');
    const noMiddleNameCheckbox = document.getElementById('no-middle-name');
    const regErrorMessage = document.getElementById('reg-error-message');
    const modal = document.getElementById("developers-modal");
    const btn = document.getElementById("about-developers");
    localStorage.removeItem('selectedCarriageType')
    localStorage.removeItem('selectedCarriages')


    loadCities();

    // Получаем элементы дат
    const departureDateInput = document.getElementById('departureTime');
    const arrivalDateInput = document.getElementById('arrivalTime');
    const today = new Date().toISOString().split('T')[0];
    // Устанавливаем минимальную дату (сегодня) для поля отправления
    function setMinDate() {
        departureDateInput.min = today;
	arrivalDateInput.min = today;
    }

    // Обработчик изменения даты отправления
    departureDateInput.addEventListener('change', function() {
        if (this.value) {
            // Устанавливаем минимальную дату прибытия = дате отправления
            arrivalDateInput.min = this.value;
            
            // Если дата прибытия была раньше - сбрасываем ее
            if (arrivalDateInput.value && arrivalDateInput.value < this.value) {
                arrivalDateInput.value = '';
            }
        }
	else {
	    arrivalDateInput.min = today;
	}
    });

    // Инициализируем минимальную дату при загрузке
    setMinDate();
    // Обработчик события ввода номера
    phoneInput.addEventListener('input', function(event) {
        const input = event.target;
        const value = input.value.replace(/\D/g, ''); // Удаляем всё, кроме цифр
        let formattedValue = ' ';

        // Форматируем значение
        if (value.length > 0) {
        formattedValue += '+7 ';

            if (value.length > 1) {
                formattedValue += '(' + value.substring(1, 4);
            }
            if (value.length > 4) {
                formattedValue += ') ' + value.substring(4, 7);
            }
            if (value.length > 7) {
                formattedValue += '-' + value.substring(7, 9);
            }
            if (value.length > 9) {
                formattedValue += '-' + value.substring(9, 11);
            }
        }

        // Устанавливаем отформатированное значение
        input.value = formattedValue;
    });

    // Обработчик события потери фокуса (если нужно добавить недостающие символы)
    phoneInput.addEventListener('blur', function(event) {
        const input = event.target;
        const value = input.value.replace(/\D/g, ''); // Удаляем всё, кроме цифр

        // Если номер неполный, добавляем недостающие символы
        if (value.length > 1 && value.length < 11) {
            let formattedValue = '+7 ';
            if (value.length > 1) {
                formattedValue += '(' + value.substring(1, 4);
            }
            if (value.length > 4) {
                formattedValue += ') ' + value.substring(4, 7);
            }
            if (value.length > 7) {
                formattedValue += '-' + value.substring(7, 9);
            }
            if (value.length > 9) {
                formattedValue += '-' + value.substring(9, 11);
            }
            input.value = formattedValue;
        }
    });

    // Обработчик изменения состояния чекбокса
    noMiddleNameCheckbox.addEventListener('change', function() {
        if (this.checked) {
            document.getElementById('middle-name').disabled = true; // Делаем поле неактивным
            document.getElementById('middle-name').value = ''; // Очищаем поле
        } else {
            document.getElementById('middle-name').disabled = false; // Делаем поле активным
        }
    });
    // Проверяем, авторизован ли пользователь
    const userLogin = localStorage.getItem('userLogin');
    const userGuid = localStorage.getItem('userGuid');

    if (userLogin && userGuid) {
        // Пользователь авторизован, показываем информацию о нем
        loginBtn.style.display = 'none';
        userInfo.style.display = 'inline';
        usernameSpan.textContent = userLogin;
    }

    // Открытие модального окна авторизации
    loginBtn.addEventListener('click', () => {
        loginModal.style.display = 'block';
    });

    // Переключение между формами авторизации и регистрации
    switchToRegister.addEventListener('click', (e) => {
        e.preventDefault();
        loginModal.style.display = 'none';
        registerModal.style.display = 'block';
    });

    switchToLogin.addEventListener('click', (e) => {
        e.preventDefault();
        registerModal.style.display = 'none';
        loginModal.style.display = 'block';
    });

    // Закрытие модальных окон
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            loginModal.style.display = 'none';
            registerModal.style.display = 'none';
            modal.style.display = 'none';
        });
    });
    btn.onclick = function(e) {
        e.preventDefault();
        modal.style.display = "block";
    }

    // Обработка формы авторизации
    document.getElementById('auth-form').addEventListener('submit', function(event) {
        event.preventDefault();
	
        loginModal.querySelectorAll('input').forEach(input => input.classList.remove('error'));
        errorMessage.textContent = '';
        errorMessage.style.display = 'none';

        const login = document.getElementById('login').value;
        const password = document.getElementById('password').value;

        if (login === 'admin' && password === 'admin') {
        	// Сохраняем информацию о том, что это администратор
        	localStorage.setItem('userLogin', login);
        
        	loginModal.style.display = 'none';
        	loginBtn.style.display = 'none';
        	userInfo.style.display = 'inline';
        	usernameSpan.textContent = login;

        	window.location.href = 'personal-cabinet-admin.html';
        	return;
	} 
        fetch('http://localhost:8081/api/v1/user/auth', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ login, password })
        })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(`Ошибка: ${text}`);
                });
            }
            return response.json();
        })
        .then(data => {
            // Сохраняем логин и GUID в localStorage
            localStorage.setItem('userLogin', login);
            localStorage.setItem('userGuid', data.token); // Предполагаем, что сервер возвращает GUID

            alert('Авторизация успешна!');
            loginModal.style.display = 'none';
            loginBtn.style.display = 'none';
            userInfo.style.display = 'inline';
            usernameSpan.textContent = login;
        })
        .catch(error => {
            document.getElementById('login').classList.add('error');
            document.getElementById('password').classList.add('error');
            errorMessage.textContent = error.message;
            errorMessage.style.display = 'block';
        });
    });

    // Обработка формы регистрации
    document.getElementById('register-form').addEventListener('submit', function(event) {
        event.preventDefault();

	registerModal.querySelectorAll('input').forEach(input => input.classList.remove('error'));
        regErrorMessage.textContent = '';
        regErrorMessage.style.display = 'none';

        const login = document.getElementById('reg-login').value;
        const password = document.getElementById('reg-password').value;
        const confirmPassword = document.getElementById('confirm-password').value;
        const email = document.getElementById('email').value;
        const phone = document.getElementById('phone').value.replace(/\D/g, '');
        const surname = document.getElementById('last-name').value;
        const name = document.getElementById('first-name').value;
        const midname = noMiddleNameCheckbox.checked ? null : document.getElementById('middle-name').value;
	
	if (password !== confirmPassword) {
            document.getElementById('reg-password').classList.add('error');
            document.getElementById('confirm-password').classList.add('error');
            regErrorMessage.textContent = 'Пароли не совпадают.';
            regErrorMessage.style.display = 'block';
            return;
        }
        fetch('http://localhost:8081/api/v1/user/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({login, email, password, phone, surname, name, midname })
        })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(`Ошибка: ${text}`);
                });
            }
            return response.json();
        })
        .then(data => {
            alert('Регистрация успешна!');
            registerModal.style.display = 'none';
            loginModal.style.display = 'block'; // Переключаем на форму авторизации
        })
        .catch(error => {
	    if (error.message == 'Ошибка: Пользователь уже существует' || error.message == 'Ошибка: Логин от 3 до 30 символов')
		document.getElementById('reg-login').classList.add('error');
 	    if (error.message == 'Ошибка: Пароль от 8 до 30 символов')
		document.getElementById('reg-password').classList.add('error');
	    if (error.message == 'Ошибка: Пользователь с таким email уже существует' || error.message == 'Ошибка: Email от 6 до 50 символов')
		document.getElementById('email').classList.add('error');
 	    if (error.message == 'Ошибка: Пользователь с таким номером телефона уже существует')
		document.getElementById('phone').classList.add('error');
	    if (error.message == 'Ошибка: Фамилия от 2 до 50 символов')
		document.getElementById('last-name').classList.add('error');
	    if (error.message == 'Ошибка: Имя от 2 до 50 символов')
		document.getElementById('first-name').classList.add('error');
            if (error.message == 'Ошибка: Номер телефона 11-значное число')
		document.getElementById('phone').classList.add('error');
            regErrorMessage.textContent = error.message;
            regErrorMessage.style.display = 'block';
        });
    });

    // Обработка выхода
    logoutBtn.addEventListener('click', function() {
        const userGuid = localStorage.getItem('userGuid');

        fetch('http://localhost:8081/api/v1/user/logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ guid: userGuid }) // Используем сохраненный GUID
        })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(`Ошибка: ${response.status} ${text}`);
                });
            }
            return response.json();
        })
        .then(data => {
            // Удаляем данные из localStorage
            localStorage.removeItem('userLogin');
            localStorage.removeItem('userGuid');

            alert('Выход выполнен!');
            loginBtn.style.display = 'inline';
            userInfo.style.display = 'none';
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert(error.message);
        });
    });

    // Обработка формы фильтров
    filterForm.addEventListener('submit', function(event) {
    event.preventDefault();

    const arrivalCityName = document.getElementById('arrivalCityName').value;
    const departureTime = document.getElementById('departureTime').value;
    const arrivalTime = document.getElementById('arrivalTime').value;

    const filterData = {
        arrivalCityName,
        departureTime: departureTime ? new Date(departureTime).toISOString() : null,
        arrivalTime: arrivalTime ? new Date(arrivalTime).toISOString() : null
    };

    fetch('http://localhost:8081/api/v1/shedule/get-filter', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(filterData)
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Ошибка: ${response.status} ${text}`);
            });
        }
        return response.json();
    })
    .then(data => {
        // Показываем таблицу с расписанием
        if (scheduleTable) {
            scheduleTable.style.display = 'table';
	    document.getElementById('schedule-warning').style.display = 'block';
        }
        // Очищаем таблицу перед добавлением новых данных
        if (scheduleTableBody) {
            scheduleTableBody.innerHTML = '';

            // Заполняем таблицу данными
            data.forEach(item => {
                const row = document.createElement('tr');
                row.classList.add('schedule-row');
                row.dataset.trainNumber = item.numberTrain;
                row.dataset.departureDate = new Date(item.departureTime).toISOString();
                row.innerHTML = `
                    <td>${item.numberTrain}</td>
                    <td>${item.departureTime}</td>
                    <td>${item.arrivalTime}</td>
                    <td>${item.departureCityName}</td>
                    <td>${item.arrivalCityName}</td>
                `;
                row.addEventListener('click', () => {
		    if (!localStorage.getItem('userLogin') || localStorage.getItem('userLogin') === 'admin') {
			alert('Для просмотра деталей и бронирования войдите в аккаунт!');
		    }
		    else {
                    // Сохраняем данные в sessionStorage
                    sessionStorage.setItem('selectedTrain', JSON.stringify({
                        numberTrain: item.numberTrain,
                        departureDate: item.departureTime
                    }));
                    
                    // Переходим на страницу с информацией о поезде
                    window.location.href = 'train-info.html';
                    }
                });
                scheduleTableBody.appendChild(row);
            });
        }
    })
    .catch(error => {
        console.error('Ошибка:', error);
        alert(error.message);
    });

});
function loadCities() {
    fetch('http://localhost:8081/api/v1/shedule/get-city')
        .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка загрузки списка городов');
            }
            return response.json();
        })
        .then(cities => {
            const sortedCities = cities
                .filter(city => city.city)
                .sort((a, b) => a.city.localeCompare(b.city));

            const datalist = document.getElementById('citiesList');
            datalist.innerHTML = '';

            sortedCities.forEach(city => {
                const option = document.createElement('option');
                option.value = city.city;
                datalist.appendChild(option);
            });

            const cityInput = document.getElementById('arrivalCityName');
            let isProgrammaticChange = false;

            // 1. При фокусе показываем ВЕСЬ список (даже если уже выбрано значение)
            cityInput.addEventListener('focus', function() {
                if (!isProgrammaticChange) {
                    this.value = ''; // Очищаем поле временно
                    setTimeout(() => {
                        this.value = this.dataset.lastValue || ''; // Возвращаем значение
                        this.setAttribute('list', 'citiesList'); // Обновляем список
                    }, 0);
                }
            });

            // 2. При выборе значения сохраняем его
            cityInput.addEventListener('change', function() {
                if (this.value) {
                    this.dataset.lastValue = this.value;
                }
            });

            // 3. Фильтрация ТОЛЬКО при ручном вводе (не при выборе из списка)
            cityInput.addEventListener('input', function() {
                if (!isProgrammaticChange) {
                    const inputValue = this.value.toLowerCase();
                    const options = datalist.querySelectorAll('option');
                    
                    options.forEach(option => {
                        option.hidden = !option.value.toLowerCase().includes(inputValue);
                    });
                }
            });

            // 4. При клике на поле - показываем весь список
            cityInput.addEventListener('click', function() {
                if (this.value) {
                    isProgrammaticChange = true;
                    this.value = '';
                    setTimeout(() => {
                        this.value = this.dataset.lastValue || '';
                        isProgrammaticChange = false;
                    }, 0);
                }
            });
        })
        .catch(error => {
            console.error('Ошибка при загрузке городов:', error);
        });
}

});