document.addEventListener('DOMContentLoaded', function() {
    const myDataLink = document.getElementById('my-data-link'); 	// Ссылка на раздел личных данных
    const passengersLink = document.getElementById('passengers-link');  // Ссылка на раздел пассажиры
    const ticketsLink = document.getElementById('bookings-section-link');// Ссылка на раздел брони
    const myDataSection = document.getElementById('my-data'); 		// Область личных данных
    const passengersSection = document.getElementById('passengers');	// Область пассажиров
    const ticketsSection = document.getElementById('bookings-section'); // Область броней
    const logoutBtn = document.getElementById('logout-btn'); 		// Кнопка выхода
    const editButtons = document.querySelectorAll('.clickable-text');   // Кнопка изменить/добавить поля ЛК
    const passengersList = document.getElementById('passengers-list');  // Список пассажиров
    const addPassengerBtn = document.getElementById('add-passenger-btn'); // Кнопка добавления пассажира
    const addPassengerForm = document.getElementById('add-passenger-form'); // Добавление пассажира
    const inputPassport = document.getElementById('passport-form');         // Поле паспорт в форме добавления пасс
    const noMiddleNameCheckbox = document.getElementById('no-midname');     // Чекбокс для отчества
    const isChildCheckbox = document.getElementById('is-child');	// Чекбокс для ребенка
    const closeModal = document.querySelector('.close');		// Кнопка закрытия модального окна
    const passengerForm = document.getElementById('passenger-form');	// Форма добавления пассажира
    const userFields = {
        'surname': document.getElementById('surname'),
        'name': document.getElementById('name'),
        'midname': document.getElementById('patronymic'),
        'email': document.getElementById('email'),
        'phone': document.getElementById('phone'),
        'date_birth': document.getElementById('birthdate'),
        'passport': document.getElementById('passport')
    };
    const warningMessage = document.getElementById('warning-message'); // Плашка
    const passErrorMessage = document.getElementById('pass-error-message'); // Ошибки для формы добавления
const btn = document.getElementById("about-developers");
const modal = document.getElementById("developers-modal");
const closeButtons = document.querySelectorAll('.close');
localStorage.removeItem('selectedCarriageType')
    localStorage.removeItem('selectedCarriages')
closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            modal.style.display = 'none';
        });
    });

btn.onclick = function(e) {
        e.preventDefault();
        modal.style.display = "block";
    }
    // Валидация паспорта в форме

    inputPassport.addEventListener('input', function(event) {
        const input = event.target;
        const value = input.value.replace(/\D/g, ''); // Удаляем всё, кроме цифр
        let formattedValue = '';

        if (value.length > 0) {
        formattedValue += value.substring(0, 4);

            if (value.length > 4) {
                formattedValue += ' ' + value.substring(4, 10);
            } 
        }

        input.value = formattedValue;
    });

    // Обработчик события потери фокуса (если нужно добавить недостающие символы)

    inputPassport.addEventListener('blur', function(event) {
        const input = event.target;
        const value = input.value.replace(/\D/g, '');

        if (value.length > 0 && value.length < 10) {
            let formattedValue = value.substring(0, 4);
            if (value.length > 4) {
                formattedValue += ' ' + value.substring(4, 10);
            }
            input.value = formattedValue;
        }
    });

    // Обработчик изменения состояния чекбокса

    noMiddleNameCheckbox.addEventListener('change', function() {
        if (this.checked) {
            console.log(document.getElementById('midname-form').value);
            document.getElementById('midname-form').disabled = true; // Делаем поле неактивным
            document.getElementById('midname-form').value = ''; // Очищаем поле
        } else {
            document.getElementById('midname-form').disabled = false; // Делаем поле активным
        }
    });

    isChildCheckbox.addEventListener('change', function() {
        if (this.checked) {
            document.getElementById('passport-form').disabled = true; // Делаем поле неактивным
            document.getElementById('passport-form').value = ''; // Очищаем поле
	    document.getElementById('email-form').disabled = true; // Делаем поле неактивным
            document.getElementById('email-form').value = ''; // Очищаем поле
        } else {
            document.getElementById('passport-form').disabled = false; // Делаем поле активным
	    document.getElementById('email-form').disabled = false; // Делаем поле активным
        }
    });

    // Маска для телефона в поле

    userFields.phone.addEventListener('input', function (e) {
        e.target.value = formatPhone(e.target.value);
    });

    // Маска для паспорта в поле

    userFields.passport.addEventListener('input', function (e) {
        e.target.value = formatPassport(e.target.value);
    });

    function formatPhone(value) {
    	if (!value) return '';
    	value = value.replace(/\D/g, ''); // Убираем все нецифровые символы
    	let formattedValue = '+7 (' + value.substring(1, 4); // +7 (XXX
    	if (value.length >= 4) {
        	formattedValue += ') ' + value.substring(4, 7); // +7 (XXX) XXX
    	}
    	if (value.length >= 7) {
        	formattedValue += '-' + value.substring(7, 9); // +7 (XXX) XXX-XX
    	}
    	if (value.length >= 9) {
        	formattedValue += '-' + value.substring(9, 11); // +7 (XXX) XXX-XX-XX
    	}
    	return formattedValue;
    }

    function formatPassport(value) {
    	if (!value) return '';
    	value = value.replace(/\D/g, ''); // Убираем все нецифровые символы
    	let formattedValue = value.substring(0, 4); // Первые 4 цифры
    	if (value.length >= 4) {
        	formattedValue += ' ' + value.substring(4, 10); // Пробел и следующие 6 цифр
    	}
    	return formattedValue;
    }

    // Установка значения для кнопки изменения поля

    setupEditButton(document.getElementById('edit-fio-text'), userFields.surname);
    setupEditButton(document.querySelector('[data-field="email"]'), userFields.email);
    setupEditButton(document.querySelector('[data-field="phone"]'), userFields.phone);
    setupEditButton(document.querySelector('[data-field="birthdate"]'), userFields.date_birth);
    setupEditButton(document.querySelector('[data-field="passport"]'), userFields.passport);

    const userLogin = localStorage.getItem('userLogin');

    function setButtonText(button, inputField) {
        const value = inputField.value;
        if (inputField.type === 'date') {
            // Для полей типа date проверяем, что значение не пустое
            button.textContent = value ? 'Изменить' : 'Добавить';
        } else {
            // Для остальных полей проверяем, что значение не null, не undefined и не пустая строка
            button.textContent = value !== null && value !== undefined  && value.trim() !== ''? 'Изменить' : 'Добавить';
        }
    }

    // Обработчик для кнопок "Изменить" и "Добавить"

    function setupEditButton(button, inputField) {
        let isEditing = false; // Флаг для отслеживания режима редактирования

        // Устанавливаем начальный текст кнопки при загрузке страницы
        setButtonText(button, inputField);

        button.addEventListener('click', function () {
            if (!isEditing) {
                // Активируем поле для редактирования
		if (button === document.getElementById('edit-fio-text'))
                {
	        	userFields.name.disabled = false;
			userFields.midname.disabled = false;
			userFields.surname.disabled = false;
                }
                inputField.disabled = false;
                button.textContent = 'Сохранить'; // Меняем текст на "Сохранить"
                isEditing = true; // Включаем режим редактирования
            } else {
                // Сохраняем данные и блокируем поле
		if (button === document.getElementById('edit-fio-text'))
                {
	        	userFields.name.disabled = true;
			userFields.midname.disabled = true;
                        userFields.surname.disabled = true;
                }
                inputField.disabled = true;
                // Проверяем, пустое поле или нет, и меняем текст кнопки
                setButtonText(button, inputField);
                updateUserData(); // Вызываем функцию обновления данных
                isEditing = false; // Выключаем режим редактирования
            }
        });
    }

    // Загружаем данные пользователя

    function loadUserData() {
        fetch(`http://localhost:8081/api/v1/user/get-user/${userLogin}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка загрузки данных пользователя');
            }
            return response.json();
        })
        .then(data => {
            // Заполняем поля формы "Мои данные"
            userFields.surname.value = data.surname || '';
            userFields.name.value = data.name || '';
            userFields.midname.value = data.midname || '';
            userFields.email.value = data.email || '';
            userFields.phone.value = formatPhone(data.phone || '');
            userFields.date_birth.value = data.date_birth ? data.date_birth.split('T')[0] : '';
            userFields.passport.value = formatPassport(data.passport || '');

            // Обновляем текст кнопок после загрузки данных
            setButtonText(document.getElementById('edit-fio-text'), userFields.surname);
            setButtonText(document.querySelector('[data-field="email"]'), userFields.email);
            setButtonText(document.querySelector('[data-field="phone"]'), userFields.phone);
            setButtonText(document.querySelector('[data-field="birthdate"]'), userFields.date_birth);
            setButtonText(document.querySelector('[data-field="passport"]'), userFields.passport);


    		if (userFields.date_birth.value && userFields.passport.value !== '') {
                warningMessage.style.display = 'none';
            }
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert('Не удалось загрузить данные пользователя');
        });
    }

    // Переключение между разделами
    myDataLink.addEventListener('click', (e) => {
        e.preventDefault();
        setActiveSection('my-data');
    });

    passengersLink.addEventListener('click', (e) => {
        e.preventDefault();
        setActiveSection('passengers');
    	loadPassengers();
    });

    ticketsLink.addEventListener('click', (e) => {
        e.preventDefault();
        setActiveSection('bookings-section');
        loadUserBookings();
    });

    // Функция для активации раздела
    function setActiveSection(sectionId) {
        // Скрываем все разделы
        myDataSection.style.display = 'none';
        passengersSection.style.display = 'none';
        ticketsSection.style.display = 'none';

        // Убираем активный класс у всех ссылок
        myDataLink.classList.remove('active');
        passengersLink.classList.remove('active');
        ticketsLink.classList.remove('active');

        // Показываем выбранный раздел и добавляем активный класс
        document.getElementById(sectionId).style.display = 'block';
        document.getElementById(`${sectionId}-link`).classList.add('active');
    }

    // Функция для сохранения данных
    async function updateUserData() {
        const login = userLogin; // Замените на реальный логин пользователя
        const data = {
            surname: userFields.surname.value.trim() || null,
            name: userFields.name.value.trim() || null,
            midname: userFields.midname.value.trim() || '',
            email: userFields.email.value || null,
            phone: userFields.phone.value.replace(/\D/g, '') || null,
            date_birth: userFields.date_birth.value || null,
            passport: userFields.passport.value.replace(/\D/g, '') || null
        };

        fetch(`http://localhost:8081/api/v1/user/update/${login}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
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
            console.log('Данные успешно сохранены:', data);
            // Скрываем плашку с предупреждением, если все данные заполнены
            if (userFields.date_birth.value && userFields.passport.value !== '') {
                warningMessage.style.display = 'none';
            }
	    else {
		warningMessage.style.display = 'block';
	    }	
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert(error.message);
	    loadUserData();
	    
        });
    }

    // По умолчанию открываем раздел "Мои данные"
    setActiveSection('my-data');
    loadUserData();

    // Обработка выхода
    logoutBtn.addEventListener('click', function() {
        const userGuid = localStorage.getItem('userGuid');

        fetch('http://localhost:8081/api/v1/user/logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ guid: userGuid })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка при выходе');
            }
            return response.json();
        })
        .then(data => {
            // Удаляем данные из localStorage
            localStorage.removeItem('userLogin');
            localStorage.removeItem('userGuid');

            // Перенаправляем на главную страницу
            window.location.href = 'index.html';
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert('Не удалось выполнить выход');
        });
    });

    // Загрузка пассажиров
    async function loadPassengers() {
        const login = userLogin; // Логин пользователя
        const response = await fetch(`http://localhost:8081/api/v1/pass/get-all-pass/${login}`);
        const passengers = await response.json();
        passengersList.innerHTML = ''; // Очистка списка
        passengers.forEach(passenger => {
            const passengerItem = document.createElement('div');
            passengerItem.className = 'passenger-item';

            const passengerHeader = document.createElement('div');
            passengerHeader.className = 'passenger-header'; // Видимая часть
            passengerHeader.innerHTML = `
                <span>Пассажир<br>${passenger.surname} ${passenger.name} ${passenger.midname || ''}</span>
                <button class="toggle-details">Данные пассажира▼</button>
                <button class="delete-passenger" data-id="${passenger.id_pass}">Удалить</button>
            `;

            const passengerDetails = document.createElement('div');
            passengerDetails.className = 'passenger-details';
            if (passenger.passport) {
            passengerDetails.innerHTML = `
                <p>Email: ${passenger.email || 'Не указан'}</p>
                <p>Паспорт: ${passenger.passport || ''}</p>
                <p>Дата рождения: ${new Date(passenger.date_birth || '').toLocaleDateString()}</p>
            `;
            } else {
                passengerDetails.innerHTML = `
                <p>Дата рождения: ${new Date(passenger.date_birth).toLocaleDateString()}</p>
                <p><strong>Пассажир является ребенком</strong></p>
            `;
        }
            passengerItem.appendChild(passengerHeader);
            passengerItem.appendChild(passengerDetails);
            passengersList.appendChild(passengerItem);
        });

        // Обработчик для разворачивания/сворачивания деталей
        document.querySelectorAll('.toggle-details').forEach(button => {
            button.addEventListener('click', function () {
                const details = this.parentElement.nextElementSibling;
                details.classList.toggle('active');
                this.textContent = details.classList.contains('active') ? 'Данные пассажира▲' : 'Данные пассажира▼';
            });
        });

        // Обработчик для удаления пассажира
        document.querySelectorAll('.delete-passenger').forEach(button => {
            button.addEventListener('click', function () {
                const id = this.getAttribute('data-id');
                if (confirm('Вы уверены, что хотите удалить пассажира? После этого все его данные будут стерты. Это действие невозможно будет отменить')) {
                    deletePassenger(id);
                }
            });
        });
    }

    // Удаление пассажира
    async function deletePassenger(id) {
        const login = userLogin;
        const response = await fetch(`http://localhost:8081/api/v1/pass/delete/${login}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id_pass: id })
        });

        if (response.ok) {
            loadPassengers(); // Перезагружаем список пассажиров
        }
    }

    // Открытие формы добавления пассажира
    addPassengerBtn.addEventListener('click', function () {
        addPassengerForm.style.display = 'block';
         document.querySelectorAll('#add-passenger-form input').forEach(input => {
         input.classList.remove('error');
    });
    });

    // Закрытие формы добавления пассажира
    closeModal.addEventListener('click', function () {
        addPassengerForm.style.display = 'none';
    });

    // Обработчик отправки формы пассажира
    passengerForm.addEventListener('submit', function(e) {
        e.preventDefault();
        passengerForm.querySelectorAll('input').forEach(input => input.classList.remove('error'));
        passErrorMessage.textContent = '';
        passErrorMessage.style.display = 'none';

        const surname = document.getElementById('surname-form').value;
        const name = document.getElementById('name-form').value;
        const midname = noMiddleNameCheckbox.checked ? null : document.getElementById('midname-form').value;
        const date_birth = document.getElementById('date_birth-form').value;
        const passport = isChildCheckbox.checked ? null : document.getElementById('passport-form').value.replace(/\D/g, '');
        const email = isChildCheckbox.checked ? null : document.getElementById('email-form').value;
        console.log(surname);
        const login = userLogin;
        fetch(`http://localhost:8081/api/v1/pass/create/${login}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({date_birth, email, passport, surname, name, midname })
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
            addPassengerForm.style.display = 'none';
            loadPassengers(); // Перезагружаем список пассажиров
        })
        .catch(error => {
            if (error.message == 'Ошибка: Пассажир с таким паспортом уже существует' || error.message == 'Ошибка: Серия и номер паспорта 10-значное число')
		document.getElementById('passport-form').classList.add('error');
	    if (error.message == 'Ошибка: Email от 6 до 50 символов' || error.message == 'Ошибка: Пассажир с таким email уже существует')
		document.getElementById('email-form').classList.add('error');
 	    if (error.message == 'Ошибка: Фамилия от 2 до 50 символов')
		document.getElementById('surname-form').classList.add('error');
	    if (error.message == 'Ошибка: Имя от 2 до 50 символов')
		document.getElementById('name-form').classList.add('error');
	    if (error.message == 'Ошибка: Проверьте дату рождения на корректность')
		document.getElementById('date_birth-form').classList.add('error');
            if (error.message == 'Ошибка: Отчество до 50 символов')
		document.getElementById('midname-form').classList.add('error');
            passErrorMessage.textContent = error.message;
            passErrorMessage.style.display = 'block';
        });
    });

   // Загрузка броней
    async function loadUserBookings() {
	    const login = userLogin;
            const bookingsList = document.getElementById('bookings-list');
            
            try {
                const response = await fetch(`http://localhost:8081/api/v1/ticket/get-all/${login}`);
                
                if (!response.ok) {
                    throw new Error(`Ошибка HTTP: ${response.status}`);
                }
                
                const bookings = await response.json();
                
                if (bookings.length === 0) {
                    bookingsList.innerHTML = '<p>У вас нет активных бронирований.</p>';
                    return;
                }
                
                renderBookings(bookings);
            } catch (error) {
                console.error('Ошибка при загрузке бронирований:', error);
                alert("Не удалось загрузить данные о бронированиях. Пожалуйста, попробуйте позже.");
            }
        }

        // Создание элемента брони
        function renderBookings(bookings) {
            const bookingsList = document.getElementById('bookings-list');
            bookingsList.innerHTML = '';
            
            bookings.forEach(booking => {
                const bookingElement = createBookingElement(booking);
                bookingsList.appendChild(bookingElement);
            });
        }

        function createBookingElement(booking) {
            const bookingElement = document.createElement('div');
            bookingElement.className = 'booking-card';
            
            const header = document.createElement('div');
            header.className = 'booking-header';
            
            const firstTicket = booking.tickets && booking.tickets.length > 0 ? booking.tickets[0] : null;
            
            const headerContent = document.createElement('div');
            headerContent.innerHTML = `
                <span class="booking-number">Бронь № ${booking.id_book}</span>
                ${firstTicket ? `
                    <span class="route-info">${firstTicket.departure_time} ${firstTicket.departure_city_name} → ${firstTicket.arrival_city_name}</span>
                ` : ''}
            `;
            
            const expandBtn = document.createElement('button');
            expandBtn.className = 'expand-btn';
            expandBtn.innerHTML = 'Просмотр билетов▼';
            expandBtn.setAttribute('aria-label', 'Раскрыть/свернуть бронь');
            
            header.appendChild(headerContent);
            header.appendChild(expandBtn);
            
            const details = document.createElement('div');
            details.className = 'booking-details';
            
            if (booking.tickets && booking.tickets.length > 0) {
                const ticketList = document.createElement('ul');
                ticketList.className = 'ticket-list';
                
                booking.tickets.forEach(ticket => {
                    const ticketItem = document.createElement('li');
                    ticketItem.className = 'ticket-item';
                    ticketItem.innerHTML = `
                        <a href="http://localhost:8081/api/v1/ticket/get-ticket/${ticket.id_ticket}/pdf" class="pdf-link" target="_blank">
                            ${ticket.passenger_name.split(' ')[0].replace(/\s+/g, ' ')}${ticket.id_ticket}.pdf
                        </a>
                        <button class="btn-cancel" data-ticket-id="${ticket.id_ticket}">
                            Отменить бронирование
                        </button>
                    `;
                    
                    const cancelBtn = ticketItem.querySelector('.btn-cancel');
                    cancelBtn.addEventListener('click', function() {
                        const ticketId = this.getAttribute('data-ticket-id');
                        cancelTicket(ticketId, userLogin);
                    });
                    
                    ticketList.appendChild(ticketItem);
                });
                
                details.appendChild(ticketList);
            } else {
                details.textContent = 'Нет информации о билетах';
            }

            expandBtn.addEventListener('click', function() {
		details.classList.toggle('expanded');
                this.innerHTML = this.innerHTML === 'Просмотр билетов▼' ? 'Просмотр билетов▲' : 'Просмотр билетов▼';
            });

            bookingElement.appendChild(header);
            bookingElement.appendChild(details);
            
            return bookingElement;
        }

	// Удаление билета
        async function cancelTicket(ticketId, login) {
            if (!confirm('Вы уверены, что хотите отменить этот билет? Это действие невозможно будет отменить!')) {
                return;
            }
            
            try {
                const response = await fetch(`http://localhost:8081/api/v1/ticket/delete-ticket/${ticketId}/${login}`, {
                    method: 'POST'
                });
                
                if (!response.ok) {
                    throw new Error(`Ошибка HTTP: ${response.status}`);
                }
                
                const result = await response.json();
                
                if (result) {
                    const login = userLogin;
                    loadUserBookings(login);
                } else {
                    throw new Error('Не удалось отменить билет');
                }
            } catch (error) {
                console.error('Ошибка при отмене билета:', error);
                alert("Не удалось отменить билет. Пожалуйста, попробуйте позже.");
            }
        }
});
