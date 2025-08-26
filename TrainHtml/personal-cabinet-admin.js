document.addEventListener('DOMContentLoaded', function() {
    const scheduleLink = document.getElementById('schedule-link');
    const vansLink = document.getElementById('vans-link');
    const trainsLink = document.getElementById('trains-link');
    const myDataSection = document.getElementById('schedule');
    const passengersSection = document.getElementById('vans');
    const ticketsSection = document.getElementById('trains');
    const btn = document.getElementById("about-developers");
    const modal = document.getElementById("developers-modal");
    const closeButtons = document.querySelectorAll('.close');
    const backToSchedule = document.getElementById('back-to-schedule');
    const logoutBtn = document.getElementById('logout-btn'); 
    // Элементы схем
    const schemaData = {
            schemaName: "",
            schemaType: "",
            compartments: [],
            sideSeats: [],
            rows: []
        };
        
        
     const addSchemaBtn = document.getElementById('addSchemaBtn');
     const schemaList = document.getElementById('schemaList');
     const statusMessage = document.getElementById('statusMessage');
     const vanConstructor = document.getElementById('van-constructor');
     const constructorTitle = document.getElementById('constructor-title');
     const cancelConstructorBtn = document.getElementById('cancelConstructor');
        
      // Элементы конструктора
     const vanTypeSelect = document.getElementById('vanType');
     const controlsContainer = document.getElementById('controls-container');
     const schemaNameInput = document.getElementById('schemaName');
     const schemaNameContainer = document.getElementById('schema-name-input');
     const visualPreview = document.getElementById('visualPreview');
     const saveButton = document.getElementById('saveSchema');
     const constructorStatus = document.getElementById('constructor-status');
        
     const trainFormContainer = document.getElementById('train-form-container');
     const trainFormTitle = document.getElementById('train-form-title');
     const trainNumberInput = document.getElementById('trainNumber');
     const trainTypeInput = document.getElementById('trainType');
     const trainNameInput = document.getElementById('trainName');
     const vanCountInput = document.getElementById('vanCount');
     const trainSaveSection = document.getElementById('train-save-section');
     const generateVansBtn = document.getElementById('generateVansBtn');
     const vansContainer = document.getElementById('vans-container');
     const saveTrainBtn = document.getElementById('saveTrainBtn');
     const cancelTrainBtn = document.getElementById('cancelTrainBtn');

     
     closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            modal.style.display = 'none';
        });
     });


    // Переключение между разделами
    scheduleLink.addEventListener('click', (e) => {
        e.preventDefault();
        setActiveSection('schedule');
        loadSchedule();
    });

    vansLink.addEventListener('click', (e) => {
        e.preventDefault();
        setActiveSection('vans');
	loadSchemas();
    });

    trainsLink.addEventListener('click', (e) => {
        e.preventDefault();
        setActiveSection('trains');
    });

    // Обработчик кнопки добавления схемы
    addSchemaBtn.addEventListener('click', function() {
    	showConstructor();
    	constructorTitle.textContent = 'Создание вагона';
    	schemaData.schemaName = "";
        schemaData.schemaType = "";
        schemaData.compartments = [];
        schemaData.sideSeats = [];
        schemaData.rows = [];
        resetConstructor();
    });

    // Обработчик кнопки отмены в конструкторе
        cancelConstructorBtn.addEventListener('click', function() {
        	hideConstructor();
        });

    // Обработчики конструктора
        vanTypeSelect.addEventListener('change', initVanType);
        saveButton.addEventListener('click', saveSchema);

    // Функция для активации раздела
    function setActiveSection(sectionId) {
        // Скрываем все разделы
        myDataSection.style.display = 'none';
        passengersSection.style.display = 'none';
        ticketsSection.style.display = 'none';

        // Убираем активный класс у всех ссылок
        scheduleLink.classList.remove('active');
        vansLink.classList.remove('active');
        trainsLink.classList.remove('active');

        // Показываем выбранный раздел и добавляем активный класс
        document.getElementById(sectionId).style.display = 'block';
        document.getElementById(`${sectionId}-link`).classList.add('active');
    }

    // Обработчик для раздела поездов
    trainsLink.addEventListener('click', (e) => {
        e.preventDefault();
    	setActiveSection('trains');
    	loadTrains();
    });



    // Функция загрузки списка поездов
    function loadTrains() {
    	fetch('http://localhost:8081/api/v1/train/get-trains')
        	.then(response => response.json())
        	.then(trains => {
            	displayTrains(trains);
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert(error.message);
        });
     }

    // Отображение списка поездов
    function displayTrains(trains) {
        trainsList.innerHTML = '';
    
        if (trains.length === 0) {
            trainsList.innerHTML = '<p>Нет доступных поездов</p>';
            return;
        }
    
        trains.forEach(train => {
        	const trainDiv = document.createElement('div');
        	trainDiv.className = 'train-item';
        	trainDiv.dataset.number = train.trainNumber;
        
        	trainDiv.innerHTML = `
            	<div class="train-info">
                	<span class="train-number">Поезд №${train.trainNumber} ${train.trainName || ''}</span>
            	</div>
           	<div class="train-actions">
                	<button class="edit-btn" data-number="${train.trainNumber}">✏️</button>
                	<button class="delete-btn" data-number="${train.trainNumber}">🗑️</button>
            	</div>
        	`;
        
        	trainsList.appendChild(trainDiv);
       });
    
       // Добавляем обработчики для кнопок
       document.querySelectorAll('.edit-btn').forEach(btn => {
        	btn.addEventListener('click', (e) => {
            		const trainNumber = e.target.dataset.number;
            		editTrain(trainNumber);
        	});
       });
    
       document.querySelectorAll('.delete-btn').forEach(btn => {
        	btn.addEventListener('click', (e) => {
            		const trainNumber = e.target.dataset.number;
            		deleteTrain(trainNumber);
        	});
       });
    }

    function loadTrainTypes() {
    	return fetch('http://localhost:8081/api/v1/train/get-type-trains')
        .then(response => response.json())
        .then(types => {
            const select = document.getElementById('trainType');
            select.innerHTML = '<option value="">Выберите тип</option>';

            types.forEach(type => {
                const option = document.createElement('option');
                option.value = type;
                option.textContent = type;
                select.appendChild(option);
            });
            
            return types;
        })
        .catch(error => {
            console.error('Error loading train types:', error);
            const select = document.getElementById('trainType');
            select.innerHTML = '<option value="">Ошибка загрузки типов</option>';
            return [];
        });
    }

   // Добавление нового поезда
   addTrainBtn.addEventListener('click', () => {
    	trainFormTitle.textContent = 'Добавление поезда';
    	trainFormContainer.style.display = 'block';
    	trainsList.style.display = 'none';
    	addTrainBtn.style.display = 'none';
    	trainSaveSection.style.display = 'none';
       
    
    // Очищаем форму
    vanCountInput.value = '2';
    trainNumberInput.value = '';
    trainNameInput.value = '';
    vansContainer.innerHTML = '';
     
    loadTrainTypes();

    // Сбрасываем сохраненные данные
    delete trainFormContainer.dataset.originalNumber;
   });

   // Генерация полей для вагонов
   generateVansBtn.addEventListener('click', () => {
    	const count = parseInt(vanCountInput.value);
    	// Валидация количества вагонов
    if (isNaN(count) || count < 2 || count > 24) {
        alert('Количество вагонов должно быть от 2 до 24');
        return;
    }
    
    	vansContainer.innerHTML = '';
    
    	for (let i = 1; i <= count; i++) {
        	const vanDiv = document.createElement('div');
        	vanDiv.className = 'van-item';
        
        vanDiv.innerHTML = `
            <div class="van-info">
                <span class="van-number">Вагон ${i}</span>
            </div>
            <div class="van-actions">
                <select class="van-schema-select" id="schemaId${i}" required>
                    <option value="">Загрузка схем...</option>
                </select>
                <button class="view-schema-btn" data-van="${i}">Просмотр схемы</button>
            </div>
        `;
        
        vansContainer.appendChild(vanDiv);
        
        // Загружаем список схем для этого вагона
        loadSchemasForVan(i);
    }
    
    // Показываем секцию сохранения
    trainSaveSection.style.display = 'block';
    });

   // Загрузка списка схем для вагона
   function loadSchemasForVan(vanIndex, currentSchemaId = null) {
        fetch('http://localhost:8081/api/v1/train/get-all-schema')
        .then(response => response.json())
        .then(schemas => {
            const select = document.getElementById(`schemaId${vanIndex}`);
            select.innerHTML = '<option value="">Выберите схему</option>';
            
            schemas.forEach(schema => {
                let schemaData;
                try {
                    schemaData = typeof schema.schema === 'string' ? 
                        JSON.parse(schema.schema) : schema.schema;
                } catch (e) {
                    console.error('Ошибка парсинга схемы:', e);
                    return;
                }
                
                const option = document.createElement('option');
                option.value = schema.id_schema;
                option.textContent = schemaData.schemaName || `Схема ${schema.id_schema}`;
                select.appendChild(option);

		// Устанавливаем текущую схему как выбранную, если она есть
            if (currentSchemaId && schema.id_schema == currentSchemaId) {
                select.value = currentSchemaId;
            }
            });
            
            // Добавляем обработчик для кнопки просмотра схемы
            document.querySelector(`.view-schema-btn[data-van="${vanIndex}"]`)
                .addEventListener('click', (e) => {
                    e.preventDefault();
                    const schemaId = document.getElementById(`schemaId${vanIndex}`).value;
                    if (schemaId) {
                        viewSchema(schemaId);
                    }
                });
        })
        .catch(error => {
            console.error('Error loading schemas:', error);
            const select = document.getElementById(`schemaId${vanIndex}`);
            select.innerHTML = '<option value="">Ошибка загрузки схем</option>';
        });
   }

   // Просмотр схемы вагона
   function viewSchema(schemaId) {
    fetch(`http://localhost:8081/api/v1/train/get-schema/${schemaId}`)
        .then(response => response.json())
        .then(schemaResponse => {
            let schemaData;
            try {
                schemaData = typeof schemaResponse.schema === 'string' ? 
                    JSON.parse(schemaResponse.schema) : schemaResponse.schema;
            } catch (e) {
                console.error('Ошибка парсинга схемы:', e);
                throw new Error('Неверный формат данных схемы');
            }
            
            // Создаем модальное окно для просмотра схемы
            const modal = document.createElement('div');
            modal.className = 'modal-admin';
            modal.style.display = 'block';
            
            modal.innerHTML = `
                <div class="modal-content-admin">
                    <span class="close-modal">&times;</span>
                    <h3>${schemaData.schemaName || 'Просмотр схемы'}</h3>
                    <p>Тип: ${schemaData.schemaType}</p>
                    <div class="schema-preview">
                        ${generateSchemaPreviewHTML(schemaData)}
                    </div>
                </div>
            `;
            
            document.body.appendChild(modal);
            
            // Обработчик закрытия модального окна
            modal.querySelector('.close-modal').addEventListener('click', () => {
                modal.remove();
            });
        })
        .catch(error => {
            console.error('Error viewing schema:', error);
            showStatus('Ошибка при просмотре схемы', 'error');
        });
    }

    // Генерация HTML для предпросмотра схемы
    function generateSchemaPreviewHTML(schemaData) {
    let html = '';
    
    if (schemaData.schemaType === "Купе" || schemaData.schemaType === "СВ" || schemaData.schemaType === "Плацкарт") {
        if (schemaData.compartments && schemaData.compartments.length > 0) {
            html += "<div class='compartments-preview'>";
            
            for (const comp of schemaData.compartments) {
                html += `<div class="compartment-preview">`;
                html += `<p>Купе №${comp.compartmentNumber}</p>`;
                html += `<div class="seats-horizontal">`;
                
                let sortedSeats;
                if (comp.seats.length === 4) {
                    sortedSeats = [...comp.seats].sort((a, b) => a % 2 - b % 2 || a - b);
                } else {
                    sortedSeats = [...comp.seats].sort((a, b) => a - b);
                }
                
                sortedSeats.forEach(seatNum => {
                    const isUpper = comp.seats.length === 4 ? seatNum % 2 === 0 : false;
                    html += `
                        <div class="seat-preview ${isUpper ? 'upper-seat' : 'lower-seat'}">
                            ${seatNum}
                        </div>
                    `;
                });
                
                html += "</div></div>";
            }
            
            html += "</div>";
        }
        
        if (schemaData.schemaType === "Плацкарт" && schemaData.sideSeats && schemaData.sideSeats.length > 0) {
            html += "<div class='side-seats-preview'>";
            
            html += "<div class='side-seats-pairs-preview'>";
            
            // Сортируем боковые места и группируем по парам
            const sortedSideSeats = [...schemaData.sideSeats].sort((a, b) => a - b);
            
            for (let i = 0; i < sortedSideSeats.length; i += 2) {
                const pair = sortedSideSeats.slice(i, i + 2);
                html += `<div class="side-seat-pair-preview">`;
                
                pair.forEach(seatNum => {
                    const isUpper = seatNum % 2 === 0;
                    html += `
                        <div class="side-seat-preview ${isUpper ? 'upper-seat' : 'lower-seat'}">
                            ${seatNum}
                        </div>
                    `;
                });
                
                html += `</div>`;
            }
            
            html += "</div></div>";
        }
    } else if (schemaData.schemaType === "Сидячий") {
        html += "<div class='rows-preview'>";
        
        for (const row of schemaData.rows) {
            html += `<div class="row-preview">`;
            html += `<p>Ряд №${row.rowNumber}</p>`;
            
            html += "<div class='left-seats-preview'>";
            for (const seat of row.leftSeats) {
                html += `<div class="seat-preview">${seat}</div>`;
            }
            html += "</div>";
            
            html += "<div class='right-seats-preview'>";
            for (const seat of row.rightSeats) {
                html += `<div class="seat-preview">${seat}</div>`;
            }
            html += "</div>";
            
            html += "</div>";
        }
        
        html += "</div>";
    }
    
    return html || "<p>Нет данных для отображения</p>";
   }

   // Редактирование поезда
   function editTrain(trainNumber) {
    fetch(`http://localhost:8081/api/v1/train/check-update?numberTrain=${trainNumber}`, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(canEdit => {
        if (canEdit) {
            alert('Этот поезд нельзя редактировать - уже назначен на рейс');
            return;
        }
        
        // Если проверка пройдена, загружаем данные поезда
        fetch(`http://localhost:8081/api/v1/train/get-trains`)
            .then(response => response.json())
            .then(trains => {
                const train = trains.find(t => t.trainNumber == trainNumber);
                if (!train) {
                    throw new Error('Поезд не найден');
                }
                
                trainFormTitle.textContent = 'Редактирование поезда';
                trainFormContainer.style.display = 'block';
                trainsList.style.display = 'none';
                addTrainBtn.style.display = 'none';
                trainSaveSection.style.display = 'block';
                
                // Заполняем данные поезда
                trainNumberInput.value = train.trainNumber;
                trainNameInput.value = train.trainName || '';
                loadTrainTypes().then(() => {
                    if (train.trainType) {
                        const typeSelect = document.getElementById('trainType');
                        typeSelect.value = train.trainType;
                    }
                });
                // Сохраняем оригинальный номер поезда
                trainFormContainer.dataset.originalNumber = train.trainNumber;
                
                // Генерируем поля для вагонов
                const vanCount = train.vans ? train.vans.length : 0;
                vanCountInput.value = vanCount;
                vansContainer.innerHTML = '';
                
                if (vanCount > 0) {
                    for (let i = 0; i < vanCount; i++) {
                        const van = train.vans[i];
                        const vanDiv = document.createElement('div');
                        vanDiv.className = 'van-item';
                        
                        vanDiv.innerHTML = `
                            <div class="van-info">
                                <span class="van-number">Вагон ${i + 1}</span>
                            </div>
                            <div class="van-actions">
                                <select class="van-schema-select" id="schemaId${i + 1}" required>
                                    <option value="">Загрузка схем...</option>
                                </select>
                                <button class="view-schema-btn" data-van="${i + 1}">Просмотр схемы</button>
                            </div>
                        `;
                        
                        vansContainer.appendChild(vanDiv);
                        
                        // Загружаем список схем для этого вагона
                        loadSchemasForVan(i + 1, van.schemaId);
                    }
                }
            })
            .catch(error => {
                console.error('Error loading train:', error);
                alert('Ошибка загрузки данных поезда', 'error');
            });
    })
    .catch(error => {
        console.error('Error checking train update:', error);
        alert('Ошибка проверки возможности редактирования');
    });
}
  
    // Сохранение поезда
    saveTrainBtn.addEventListener('click', () => {
    const trainNumber = parseInt(trainNumberInput.value);
    if (isNaN(trainNumber)) {
        return;
    } 
    
    const trainType = trainTypeInput.value;
    const trainName = trainNameInput.value || null;

    // Собираем данные о вагонах
    const vanCount = parseInt(vanCountInput.value);
    const vans = [];
    let isValid = true;
    
    for (let i = 1; i <= vanCount; i++) {
        const schemaId = parseInt(document.getElementById(`schemaId${i}`).value);
        
        if (isNaN(schemaId)) {
            showStatus(`Выберите схему для вагона ${i}`, 'error');
            isValid = false;
            break;
        }
        
        vans.push({
            vanNumber: i, // Номер вагона по порядку
            schemaId: schemaId
        });
    }
    
    if (!isValid) return;
    
    const trainData = {
        trainNumber: trainNumber,
        trainType: trainType,
        trainName: trainName,
        vans: vans
    };
    
    // Определяем, создаем новый поезд или обновляем существующий
    const isEdit = trainFormContainer.dataset.originalNumber !== undefined;
    const originalNumber = trainFormContainer.dataset.originalNumber;
    
    const url = isEdit ? 
        'http://localhost:8081/api/v1/train/update-train' : 
        'http://localhost:8081/api/v1/train/create-train';
    
    const method = isEdit ? 'PUT' : 'POST';
    
    // Для редактирования добавляем оригинальный номер
    if (isEdit) {
        trainData.originalTrainNumber = parseInt(originalNumber);
    }
    
    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(trainData)
    })
    .then(response => response.json())
    .then(result => {
        if (result) {
            alert(`Поезд успешно ${isEdit ? 'обновлен' : 'добавлен'}`, 'success');
            loadTrains();
            cancelTrainForm();
        } else {
            trainSaveError.style.display = 'block';
            throw new Error('Ошибка сохранения');
        }
    })
    .catch(error => {
        alert('Ошибка сохранения');
        });
    });

    // Удаление поезда
    function deleteTrain(trainNumber) {
    // Сначала проверяем можно ли удалять
    fetch(`http://localhost:8081/api/v1/train/check-update?numberTrain=${trainNumber}`, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(canDelete => {
        if (canDelete) {
            alert('Этот поезд нельзя удалить, так как он есть в активных расписаниях');
            return;
        }

        if (!confirm(`Вы уверены, что хотите удалить поезд №${trainNumber}?`)) {
            return;
        }
        
        fetch(`http://localhost:8081/api/v1/train/delete-train/${trainNumber}`, {
            method: 'POST'
        })
        .then(response => response.json())
        .then(result => {
            if (result) {
                loadTrains();
            } else {
                throw new Error('Ошибка удаления');
            }
        })
        .catch(error => {
            alert(error.message);
        });
    })
    .catch(error => {
        console.error('Ошибка проверки:', error);
        alert('Не удалось проверить возможность удаления');
    });
}

// Отмена редактирования/добавления
cancelTrainBtn.addEventListener('click', cancelTrainForm);

function cancelTrainForm() {
    trainFormContainer.style.display = 'none';
    trainsList.style.display = 'block';
    addTrainBtn.style.display = 'block';
    trainSaveSection.style.display = 'none';
    
    // Скрываем сообщения об ошибках
    
}
    // Загрузка расписания
    function loadSchedule() {
        fetch('http://localhost:8081/api/v1/shedule/get-all')
            .then(response => response.json())
            .then(data => {
                renderScheduleTable(data);
            })
            .catch(error => {
                myDataSection.innerHTML = '<p>Ошибка загрузки расписания</p>';
            });
    }

    // Загрузка городов
    function loadCities() {
        return fetch('http://localhost:8081/api/v1/shedule/get-city')
            .then(response => response.json())
            .then(cities => {
            // Фильтруем и сортируем города
            const sortedCities = cities
                .filter(city => city.city) // Отфильтровываем пустые значения
                .sort((a, b) => a.city.localeCompare(b.city)); // Сортировка по алфавиту
            
            return sortedCities;
            })
            .catch(error => {
                return [];
            });
    }

    // Загрузка маршрутов (город-город)
    function loadRoutes() {
        return fetch('http://localhost:8081/api/v1/shedule/get-routes') // Предполагаемый endpoint
            .then(response => response.json())
	    .then(routes => {
            // Сортируем маршруты по алфавиту
            return routes
                .filter(route => route.route)
                .sort((a, b) => a.route.localeCompare(b.route));
            })
            .catch(error => {
                return [];
            });
    }

    // Загрузка номеров поездов
    function loadTrainNumbers() {
    return fetch('http://localhost:8081/api/v1/train/get-van-number')
        .then(response => response.json())
        .then(numbers => {
            // Преобразуем массив чисел в массив объектов для единообразия
            return numbers.map(number => ({ number: number }));
        })
        .catch(error => {
            return [];
        });
     }

    // Отрисовка таблицы расписания
    function renderScheduleTable(schedules) {
    Promise.all([loadCities(), loadRoutes(), loadTrainNumbers()])
    .then(([cities, routes, trainNumbers]) => {
        const cityList = cities.map(city => city.city).filter(city => city);
        const routeList = routes.map(route => route.route).filter(route => route);
        const trainNumberList = trainNumbers.map(train => train.number).filter(num => num !== undefined);
        
        myDataSection.innerHTML = `
            <div class="schedule-container">
                <div class="schedule-table-container" id="schedule-table-container">
                    <table class="schedule-table-admin">
                        <thead>
                            <tr>
                                <th>Номер поезда</th>
                                <th>Маршрут</th>
                                <th>Дата отправления</th>
                                <th>Дата прибытия</th>
                                <th>Действия</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${schedules.map(schedule => `
                                <tr data-id="${schedule.numberTrain}" data-departure="${schedule.departureTime}">
                                    <td>${schedule.numberTrain}</td>
                                    <td>${schedule.departureCityName || ''} - ${schedule.arrivalCityName || ''}</td>
                                    <td>${formatDate(schedule.departureTime)} ${formatTime(schedule.departureTime)}</td>
                                    <td>${formatDate(schedule.arrivalTime)} ${formatTime(schedule.arrivalTime)}</td>
                                    <td class="actions">
                                        <button class="edit-btn" title="Редактировать">✏️</button>
                                        <button class="delete-btn" title="Удалить">🗑️</button>
                                    </td>
                                </tr>
                            `).join('')}
                        </tbody>
                    </table>
                </div>
                
                <div class="edit-form-container" id="edit-form-container" style="display: none;">
                    <h3 id="form-title">Добавить новый рейс</h3>
                    <form id="schedule-form">
                        <div class="form-row">
                            <div class="form-group">
                                <label for="route">Маршрут:</label>
                                <select id="route" name="route" required>
                                    <option value="">Выберите маршрут</option>
                                    ${routeList.map(route => `<option value="${route}">${route}</option>`).join('')}
                                </select>
                            </div>
                            <div class="form-group">
                                <label for="numberTrainNew">Поезд:</label>
                                <select id="numberTrainNew" name="numberTrainNew" required>
                                    <option value="">Выберите поезд</option>
                                    ${trainNumberList.map(num => `<option value="${num}">Поезд №${num}</option>`).join('')}
                                </select>
                            </div>
                        </div>
                        
                        <div class="form-row">
                            <div class="form-group">
                                <label for="departureDate">Дата отправления:</label>
                                <input type="date" id="departureDate" name="departureDate" required>
                            </div>
                            <div class="form-group">
                                <label for="departureTime">Время отправления:</label>
                                <input type="time" id="departureTime" name="departureTime" required>
                            </div>
                        </div>
                        
                        <div class="form-row">
                            <div class="form-group">
                                <label for="arrivalDate">Дата прибытия:</label>
                                <input type="date" id="arrivalDate" name="arrivalDate" required>
                            </div>
                            <div class="form-group">
                                <label for="arrivalTime">Время прибытия:</label>
                                <input type="time" id="arrivalTime" name="arrivalTime" required>
                            </div>
                        </div>
                        
                        <div class="form-actions">
                            <button type="submit" class="save-btn">Сохранить</button>
                            <button type="button" id="cancel-form-btn" class="cancel-btn">Отмена</button>
                        </div>
                    </form>
                </div>
            </div>
            
            <div class="schedule-actions">
                <button id="add-schedule-btn" class="add-btn-route">Добавить новый рейс</button>
            </div>
        `;
        // Получаем элементы дат
const departureDateInput = document.getElementById('departureDate');
const departureTimeInput = document.getElementById('departureTime');
const arrivalDateInput = document.getElementById('arrivalDate');
const arrivalTimeInput = document.getElementById('arrivalTime');
const today = new Date().toISOString().split('T')[0];

// Устанавливаем минимальную дату (сегодня) для полей
function initDateInputs() {
    departureDateInput.min = today;
    arrivalDateInput.min = today;
    
    // Очищаем значения при загрузке
    departureDateInput.value = '';
    departureTimeInput.value = '';
    arrivalDateInput.value = '';
    arrivalTimeInput.value = '';
}

// Обработчик изменения даты отправления
departureDateInput.addEventListener('change', function() {
    if (this.value) {
        // Устанавливаем минимальную дату прибытия = дате отправления
        arrivalDateInput.min = this.value;
        
        // Если выбрана дата прибытия раньше - сбрасываем ее
        if (arrivalDateInput.value && arrivalDateInput.value < this.value) {
            arrivalDateInput.value = '';
        }
        
        // Если даты одинаковые - проверяем время
        if (arrivalDateInput.value === this.value) {
            validateTimeInputs();
        }
    } else {
        arrivalDateInput.min = today;
    }
});

// Обработчик изменения времени отправления
departureTimeInput.addEventListener('change', validateTimeInputs);

// Обработчик изменения времени прибытия
arrivalTimeInput.addEventListener('change', validateTimeInputs);

// Валидация времени
function validateTimeInputs() {
    if (departureDateInput.value === arrivalDateInput.value && 
        departureDateInput.value && 
        arrivalDateInput.value) {
        
        // Если даты одинаковые, проверяем время
        if (departureTimeInput.value && arrivalTimeInput.value) {
            const departure = new Date(`${departureDateInput.value}T${departureTimeInput.value}`);
            const arrival = new Date(`${arrivalDateInput.value}T${arrivalTimeInput.value}`);
            
            if (arrival <= departure) {
                // Делаем время прибытия недоступным для выбора
                arrivalTimeInput.min = departureTimeInput.value;
                
                // Если время уже выбрано и невалидно - сбрасываем
                if (arrivalTimeInput.value && arrival <= departure) {
                    arrivalTimeInput.value = '';
                }
            }
        }
    } else {
        arrivalTimeInput.removeAttribute('min');
    }
}

// Инициализация при загрузке
initDateInputs();
        // Обработчик кнопки добавления
        document.getElementById('add-schedule-btn').addEventListener('click', () => {
            document.getElementById('schedule-table-container').style.display = 'none';
            document.getElementById('edit-form-container').style.display = 'block';
            document.getElementById('form-title').textContent = 'Добавить новый рейс';
	    resetForm();
        });

        // Обработчик кнопки отмены
        document.getElementById('cancel-form-btn').addEventListener('click', () => {
            document.getElementById('edit-form-container').style.display = 'none';
            document.getElementById('schedule-table-container').style.display = 'block';
        });

        // Обработчик формы
        document.getElementById('schedule-form').addEventListener('submit', (e) => {
            e.preventDefault();
            if (document.getElementById('form-title').textContent === 'Добавить новый рейс') {
                createSchedule();
            } else {
                updateSchedule();
            }
        });

        // Добавляем обработчики для кнопок редактирования
        document.querySelectorAll('.edit-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const row = e.target.closest('tr');
                const numberTrain = row.getAttribute('data-id');
                const departureTime = row.getAttribute('data-departure');
                const schedule = schedules.find(s => 
                    s.numberTrain == numberTrain && s.departureTime == departureTime);
                
                document.getElementById('schedule-table-container').style.display = 'none';
                document.getElementById('edit-form-container').style.display = 'block';
                document.getElementById('form-title').textContent = 'Редактировать рейс';
                fillEditForm(schedule, numberTrain, departureTime);
            });
        });
       
        // Добавляем обработчики для кнопок удаления
        document.querySelectorAll('.delete-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const row = e.target.closest('tr');
                const numberTrain = row.getAttribute('data-id');
                const departureTime = row.getAttribute('data-departure');
                if (confirm('Вы уверены, что хотите удалить это расписание?')) {
                    deleteSchedule(numberTrain, departureTime);
                }
            });
        });
    });
}

// Сброс формы
function resetForm() {
    document.getElementById('schedule-form').reset();
    delete document.getElementById('schedule-form').dataset.originalNumberTrain;
    delete document.getElementById('schedule-form').dataset.originalDepartureTime;
}
// Заполнение формы редактирования
    function fillEditForm(schedule, numberTrain, departureTime) {
        const departureDate = new Date(schedule.departureTime);
        const arrivalDate = new Date(schedule.arrivalTime);

// Корректируем даты для отображения в форме
    const localDepartureDate = new Date(departureDate.getTime() - (departureDate.getTimezoneOffset() * 60000));
    const localArrivalDate = new Date(arrivalDate.getTime() - (arrivalDate.getTimezoneOffset() * 60000));

        document.getElementById('route').value = `${schedule.departureCityName}-${schedule.arrivalCityName}`;
        document.getElementById('numberTrainNew').value = schedule.numberTrain;
        document.getElementById('departureDate').value = localDepartureDate.toISOString().split('T')[0];
        document.getElementById('departureTime').value = departureDate.toTimeString().substring(0, 5);
        document.getElementById('arrivalDate').value = localArrivalDate.toISOString().split('T')[0];
        document.getElementById('arrivalTime').value = arrivalDate.toTimeString().substring(0, 5);
        
        // Сохраняем оригинальные данные для обновления
        document.getElementById('schedule-form').dataset.originalNumberTrain = numberTrain;
        document.getElementById('schedule-form').dataset.originalDepartureTime = departureTime;

    }

   // Форматирование даты
    function formatDate(dateTimeString) {
        if (!dateTimeString) return '';
        const date = new Date(dateTimeString);
        return date.toLocaleDateString('ru-RU');
    }

    // Форматирование времени
    function formatTime(dateTimeString) {
        if (!dateTimeString) return '';
        const date = new Date(dateTimeString);
        return date.toLocaleTimeString('ru-RU', {hour: '2-digit', minute:'2-digit'});
    }

// Создание нового расписания
function createSchedule() {
    const route = document.getElementById('route').value;
    const [departureCity, arrivalCity] = route.split('-');
    
    const formData = {
        numberTrainNew: parseInt(document.getElementById('numberTrainNew').value),
        departureTimeNew: `${document.getElementById('departureDate').value}T${document.getElementById('departureTime').value}:00`,
        arrivalTimeNew: `${document.getElementById('arrivalDate').value}T${document.getElementById('arrivalTime').value}:00`,
        departureCityNameNew: departureCity,
        arrivalCityNameNew: arrivalCity
    };

    fetch('http://localhost:8081/api/v1/shedule/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData)
    })
    .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка: ${text}');
            }
            return response.json();
        })
    .then(data => {
        if (data) {
            alert('Расписание успешно добавлено');
            document.getElementById('edit-form-container').style.display = 'none';
            loadSchedule();
            document.getElementById('schedule-table-container').style.display = 'block';
        }
    })
    .catch(error => {
        alert(error.message);
    });
}

// Удаление расписания
    function deleteSchedule(numberTrain, departureTime) {
    const originalDate = new Date(departureTime);
    const formattedOriginalDepartureTime = new Date(originalDate.getTime() - (originalDate.getTimezoneOffset() * 60000));

        const formData = {
            number_train: parseInt(numberTrain),
            dateDeparture: formattedOriginalDepartureTime.toISOString()
        };

        fetch('http://localhost:8081/api/v1/shedule/delete', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData)
        })
        .then(response => response.json())
        .then(data => {
            if (data) {
                alert('Расписание успешно удалено');
                loadSchedule();
            }
        })
        .catch(error => {
            alert(error.message);
        });
    }

// Обновление расписания
function updateSchedule() {
    const route = document.getElementById('route').value;
    const [departureCity, arrivalCity] = route.split('-');
    const originalDepartureTime = document.getElementById('schedule-form').dataset.originalDepartureTime;
    const originalDate = new Date(originalDepartureTime);
    const formattedOriginalDepartureTime = new Date(originalDate.getTime() - (originalDate.getTimezoneOffset() * 60000));

    const formData = {
        numberTrain: parseInt(document.getElementById('schedule-form').dataset.originalNumberTrain),
        departureTime: formattedOriginalDepartureTime.toISOString(),
        numberTrainNew: parseInt(document.getElementById('numberTrainNew').value),
        departureTimeNew: `${document.getElementById('departureDate').value}T${document.getElementById('departureTime').value}:00`,
        arrivalTimeNew: `${document.getElementById('arrivalDate').value}T${document.getElementById('arrivalTime').value}:00`,
        departureCityNameNew: departureCity,
        arrivalCityNameNew: arrivalCity
    };

    fetch('http://localhost:8081/api/v1/shedule/update', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData)
    })
    .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка: ${text}');
            }
            return response.json();
        })
    .then(data => {
        if (data) {
            alert('Расписание успешно обновлено');
            document.getElementById('edit-form-container').style.display = 'none';
	                loadSchedule();
            document.getElementById('schedule-table-container').style.display = 'block';
        }
    })
    .catch(error => {
        alert(error.message);
    });
}

// Показать конструктор
        function showConstructor() {
            schemaList.style.display = 'none';
            addSchemaBtn.style.display = 'none';
            vanConstructor.style.display = 'block';
        }

        // Скрыть конструктор
        function hideConstructor() {
            schemaList.style.display = 'block';
            addSchemaBtn.style.display = 'block';
            vanConstructor.style.display = 'none';
        }

        // Сброс конструктора
        function resetConstructor() {
            vanTypeSelect.value = '';
            schemaNameInput.value = '';
            controlsContainer.style.display = 'none';
            schemaNameContainer.style.display = 'none';
            saveButton.style.display = 'none';
            visualPreview.innerHTML = '<p>Выберите тип вагона и добавьте элементы</p>';
        }

        // ========== Функции для работы со схемами ==========
        // Загрузка списка схем
        async function loadSchemas() {
            try {
                const response = await fetch('http://localhost:8081/api/v1/train/get-all-schema');
                
                if (!response.ok) {
                    throw new Error('Ошибка загрузки списка схем');
                }
                
                const schemas = await response.json();
                displaySchemas(schemas);
            } catch (error) {
                alert(error.message);
            }
        }
        
        // Отображение списка схем
        function displaySchemas(schemas) {
            // Очищаем список перед добавлением
            while (schemaList.children.length > 0) {
                schemaList.removeChild(schemaList.lastChild);
            }
            
            if (schemas.length === 0) {
                const emptyMsg = document.createElement('p');
                emptyMsg.textContent = 'Нет сохраненных схем.';
                schemaList.appendChild(emptyMsg);
                return;
            }
            
            schemas.forEach(schema => {
                const schemaItem = document.createElement('div');
                schemaItem.className = 'schema-item';
                // Парсим данные схемы из строки JSON
        let schemaData;
        try {
            schemaData = typeof schema.schema === 'string' ? JSON.parse(schema.schema) : schema.schema;
        } catch (e) {
            console.error('Ошибка парсинга схемы:', e);
            schemaData = { schemaName: 'Неизвестная схема', schemaType: 'unknown' };
        }
                const schemaInfo = document.createElement('div');
        schemaInfo.textContent = `${schemaData.schemaName || 'Без названия'}`;
        
        const schemaActions = document.createElement('div');
        schemaActions.className = 'schema-actions';
        
        const editBtn = document.createElement('button');
        editBtn.className = 'edit-btn';
        editBtn.textContent = '✏️';
        editBtn.addEventListener('click', () => editSchema(schema.id_schema, schemaData));
        
        const deleteBtn = document.createElement('button');
        deleteBtn.className = 'delete-btn';
        deleteBtn.textContent = '🗑️';
        deleteBtn.addEventListener('click', () => deleteSchema(schema.id_schema));
        
        schemaActions.appendChild(editBtn);
        schemaActions.appendChild(deleteBtn);
        
        schemaItem.appendChild(schemaInfo);
        schemaItem.appendChild(schemaActions);
        
        schemaList.appendChild(schemaItem);
            });
        }
        
        // Редактирование схемы
    function editSchema(schemaId, schemaDataFromServer = null) {
    // Сначала проверяем можно ли редактировать схему
    fetch(`http://localhost:8081/api/v1/train/check-update-schema/${schemaId}`, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(canEdit => {
        if (canEdit) {
            alert('Эту схему нельзя редактировать - она используется в активных поездах');
            return;
        }
        
        // Если проверка пройдена, продолжаем редактирование
        if (schemaDataFromServer) {
            initEditForm(schemaId, schemaDataFromServer);
            return;
        }

        // Иначе загружаем с сервера
        fetch(`http://localhost:8081/api/v1/train/get-schema/${schemaId}`)
            .then(response => response.json())
            .then(schemaResponse => {
                let parsedSchema;
                try {
                    parsedSchema = typeof schemaResponse.schema === 'string' 
                        ? JSON.parse(schemaResponse.schema) 
                        : schemaResponse.schema;
                } catch (e) {
                    console.error('Ошибка парсинга схемы:', e);
                }
                
                initEditForm(schemaId, parsedSchema);
            })
            .catch(error => {
                alert(error.message);
            });
    })
    .catch(error => {
        console.error('Ошибка проверки схемы:', error);
        alert('Не удалось проверить возможность редактирования');
    });
}

function initEditForm(schemaId, schemaDataFromServer) {
    showConstructor();
    constructorTitle.textContent = 'Редактировать схему';
    
    // Очищаем текущие данные
    schemaData.schemaName = "";
    schemaData.schemaType = "";
    schemaData.compartments = [];
    schemaData.sideSeats = [];
    schemaData.rows = [];
    
    // Заполняем данные из полученной схемы
    schemaData.schemaName = schemaDataFromServer.schemaName || '';
    schemaData.schemaType = schemaDataFromServer.schemaType || '';
    
    // Преобразуем compartments
    if (schemaDataFromServer.compartments) {
        schemaData.compartments = schemaDataFromServer.compartments.map((comp, index) => ({
            compartmentNumber: index + 1,
            seats: comp.seats || [],
            id: comp.id || Date.now() + index
        }));
    }
    
    // Преобразуем sideSeats
    if (schemaDataFromServer.sideSeats) {
        schemaData.sideSeats = [...schemaDataFromServer.sideSeats];
    }
    
    // Преобразуем rows
    if (schemaDataFromServer.rows) {
        schemaData.rows = schemaDataFromServer.rows.map((row, index) => ({
            rowNumber: index + 1,
            leftSeats: row.leftSeats || [],
            rightSeats: row.rightSeats || [],
            id: row.id || Date.now() + index
        }));
    }
    
    // Устанавливаем значения в форму
    schemaNameInput.value = schemaData.schemaName;
    vanTypeSelect.value = schemaData.schemaType;
    
    // Принудительно инициализируем тип вагона
    setTimeout(() => {
        initVanType();
        updateCounters();
        updateVisualPreview();
    }, 0);
    
    // Показываем кнопки
    saveButton.style.display = 'inline-block';
    schemaNameContainer.style.display = 'block';
    saveButton.dataset.schemaId = schemaId;
}
        // Удаление схемы
        async function deleteSchema(schemaId) {
    try {
        // Сначала проверяем можно ли удалять схему
        const checkResponse = await fetch(`http://localhost:8081/api/v1/train/check-update-schema/${schemaId}`, {
            method: 'POST'
        });
        
        const canDelete = await checkResponse.json();
        
        if (canDelete) {
            alert('Эту схему нельзя удалить - она используется в активных поездах');
            return;
        }
        
        if (!confirm('Вы уверены, что хотите удалить эту схему?')) {
            return;
        }
        
        // Если проверка пройдена, выполняем удаление
        const deleteResponse = await fetch(`http://localhost:8081/api/v1/train/delete-schema/${schemaId}`, {
            method: 'POST'
        });
        
        if (!deleteResponse.ok) {
            throw new Error('Ошибка удаления схемы');
        }
        
        const result = await deleteResponse.json();
        if (result) {
            showStatus('Схема успешно удалена', 'success');
            loadSchemas(); // Обновляем список
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Не удалось проверить возможность удаления');
    }
}
        
        // Сохранение схемы
        async function saveSchema() {
            if (!schemaNameInput.value) {
                showConstructorStatus("Ошибка: укажите название схемы", "error");
                return;
            }
            
            schemaData.schemaName = schemaNameInput.value;
            try {
                const schemaToSave = {
                    schemaName: schemaData.schemaName,
                    schemaType: schemaData.schemaType,
                    compartments: schemaData.compartments,
                    sideSeats: schemaData.sideSeats,
                    rows: schemaData.rows
                };
                
                let response;
                if (constructorTitle.textContent === 'Создание вагона') {
                    // Создание новой схемы
                    response = await fetch('http://localhost:8081/api/v1/train/save-schema', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({
                            jsonSchema: schemaToSave
                        })
                    });
                } else {
                    const schemaId = saveButton.dataset.schemaId;
                    response = await fetch(`http://localhost:8081/api/v1/train/update-schema/${schemaId}`, {
                        method: 'PUT',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({
                            jsonSchema: schemaToSave
			})
                    });
                }
                
                const data = await response.json();
                showConstructorStatus("Схема успешно сохранена!", "success");
                hideConstructor();
                loadSchemas();
            } catch (error) {
                alert(error.message);
            }
        }

        // ========== Функции конструктора вагонов ==========
        // Инициализация выбранного типа вагона
        function initVanType() {
            const vanType = vanTypeSelect.value;
            if (!vanType) {
                controlsContainer.style.display = 'none';
                schemaNameContainer.style.display = 'none';
                saveButton.style.display = 'none';
                return;
            }
             document.getElementById('addCompartment4').removeEventListener('click', addCompartment);
    document.getElementById('addCompartment2').removeEventListener('click', addCompartment);
    document.getElementById('addSVCompartment').removeEventListener('click', addCompartment);
    document.getElementById('addPlatskartCompartment4').removeEventListener('click', addCompartment);
    document.getElementById('addPlatskartCompartment2').removeEventListener('click', addCompartment);
    document.getElementById('addPlatskartSideSeats').removeEventListener('click', addSideSeats);
    document.getElementById('addSittingRow').removeEventListener('click', addSittingRow);
            schemaData.schemaType = vanType;
            
            // Показываем контролы
            controlsContainer.style.display = 'block';
            
            // Скрываем все контролы
            document.querySelectorAll('#controls-container > div').forEach(el => el.style.display = 'none');
            
            // Показываем нужные контролы
            if (vanType === 'Купе') {
                document.getElementById('coupe-controls').style.display = 'block';
                
                // Обработчики для купе
                document.getElementById('addCompartment4').addEventListener('click', () => addCompartment(4));
                document.getElementById('addCompartment2').addEventListener('click', () => addCompartment(2));
                
            } else if (vanType === 'СВ') {
                document.getElementById('sv-controls').style.display = 'block';
                
                // Обработчики для СВ
                document.getElementById('addSVCompartment').addEventListener('click', () => addCompartment(2));
                
            } else if (vanType === 'Плацкарт') {
                document.getElementById('platskart-controls').style.display = 'block';
                
                // Обработчики для плацкарта
                document.getElementById('addPlatskartCompartment4').addEventListener('click', () => addCompartment(4));
                document.getElementById('addPlatskartCompartment2').addEventListener('click', () => addCompartment(2));
                document.getElementById('addPlatskartSideSeats').addEventListener('click', addSideSeats);
                
            } else if (vanType === 'Сидячий') {
                document.getElementById('sitting-controls').style.display = 'block';
                
                // Обработчик для сидячего
                document.getElementById('addSittingRow').addEventListener('click', addSittingRow);
            }
            
            updateVisualPreview();
        }

        // Добавление купе
        function addCompartment(seatsCount) {
            // Проверка на максимальное количество купе (9)
            if (schemaData.compartments.length > 8) {
                showConstructorStatus("Ошибка: максимальное количество купе - 9", "error");
                return;
            }
            
            const lastSeat = getLastSeatNumber(false);
            const newSeats = Array.from({length: seatsCount}, (_, i) => lastSeat + i + 1);
            
            const compartment = {
                compartmentNumber: schemaData.compartments.length + 1,
                seats: newSeats,
                id: Date.now() // Уникальный ID для управления
            };
            
            schemaData.compartments.push(compartment);
            updateAllSeatNumbers();
            updateCounters();
            updateVisualPreview();
            
            // Показываем поле для названия схемы, если вагон заполнен
            checkVanCompletion();
	    
        }

        // Добавление боковых мест для плацкарта
        function addSideSeats() {
        if (schemaData.compartments.length < 7) {
        	showConstructorStatus("Ошибка: сначала добавьте 7-9 блоков основных мест", "error");
        	return;
    	}
            // Проверка на максимальное количество блоков боковых мест (9)
            const sideBlocks = Math.ceil(schemaData.sideSeats.length / 2);
            if (sideBlocks >= 9) {
                showConstructorStatus("Ошибка: максимальное количество блоков боковых мест - 9", "error");
                return;
            }
            
            const lastSeat = getLastSeatNumber(false);
            const newSeats = [lastSeat + 1, lastSeat + 2];
            
            schemaData.sideSeats.push(...newSeats);
            
            updateCounters();
            updateVisualPreview();
            
            checkVanCompletion();
        }

        // Добавление ряда для сидячего вагона
        function addSittingRow() {
            if (schemaData.rows.length > 24) {
        	showConstructorStatus("Ошибка: максимальное количество мест вагоне 100", "error");
        	return;
           }
            const lastSeat = getLastSeatNumber(false);
            const newLeftSeats = [lastSeat + 1, lastSeat + 2];
            const newRightSeats = [lastSeat + 3, lastSeat + 4];
            
            schemaData.rows.push({
                rowNumber: schemaData.rows.length + 1,
                leftSeats: newLeftSeats,
                rightSeats: newRightSeats,
                id: Date.now() // Уникальный ID для управления
            });
            
            updateVisualPreview();
            checkVanCompletion();
        }

        // Получить номер последнего места
        function getLastSeatNumber(includeSideSeats = false) {
            let lastSeat = 0;
            
            // Проверяем места в купе
            for (const comp of schemaData.compartments) {
                const maxInComp = Math.max(...comp.seats);
                if (maxInComp > lastSeat) lastSeat = maxInComp;
            }
            
            // Проверяем боковые места
            if (includeSideSeats && schemaData.sideSeats.length > 0) {
                const maxSide = Math.max(...schemaData.sideSeats);
                if (maxSide > lastSeat) lastSeat = maxSide;
            }
            
            // Проверяем места в сидячем вагоне
            for (const row of schemaData.rows) {
                const maxInRow = Math.max(...row.leftSeats, ...row.rightSeats);
                if (maxInRow > lastSeat) lastSeat = maxInRow;
            }
            
            return lastSeat;
        }

        // Обновление счетчиков
        function updateCounters() {
            const count = schemaData.compartments.length;
            document.getElementById('compartmentCounter').textContent = `Добавлено купе: ${count}/9`;
            document.getElementById('svCompartmentCounter').textContent = `Добавлено купе: ${count}/9`;
            document.getElementById('platskartCompartmentCounter').textContent = `Добавлено купе: ${count}/9`;
            
            const sideSeatsCount = schemaData.sideSeats.length;
            const sideBlocks = Math.ceil(sideSeatsCount / 2);
            document.getElementById('sideSeatsCounter').textContent = 
                `Добавлено боковых мест: ${sideSeatsCount} (блоков: ${sideBlocks}/9)`;
            checkVanCompletion();
        }

        // Проверка заполненности вагона
        function checkVanCompletion() {
            let isComplete = false;

            if (schemaData.schemaType === 'Купе' || schemaData.schemaType === 'СВ') {
                isComplete = schemaData.compartments.length > 6;
            } else if (schemaData.schemaType === 'Плацкарт') {
                const sideBlocks = Math.ceil(schemaData.sideSeats.length / 2);
                isComplete = schemaData.compartments.length > 0 && sideBlocks >= 7 && schemaData.compartments.length > 6;
            } else if (schemaData.schemaType === 'Сидячий') {
                isComplete = schemaData.rows.length >= 9;
            }
            
            if (isComplete) {
        schemaNameContainer.style.display = 'block';
        saveButton.style.display = 'inline-block';
    } else {
        schemaNameContainer.style.display = 'none';
        saveButton.style.display = 'none';
    }
        }

        // Обновление визуального предпросмотра
        function updateVisualPreview() {

            let html = "";
            
            if (schemaData.schemaType === "Купе" || schemaData.schemaType === "СВ" || schemaData.schemaType === "Плацкарт") {
    if (schemaData.compartments && schemaData.compartments.length > 0) {
        html += "<div class='compartments-container'>";
        
        for (const comp of schemaData.compartments) {
            html += `<div class="compartment" data-id="${comp.id}">`;
            html += `<div class="compartment-controls">`;
            html += `<button class="delete-btn" data-id="${comp.id}">×</button>`;
            html += `<button class="move-up-btn" data-id="${comp.id}">↑</button>`;
            html += `<button class="move-down-btn" data-id="${comp.id}">↓</button>`;
            html += `</div>`;
            
            html += `<p>Купе №${comp.compartmentNumber}</p>`;
            html += `<div class="seats-horizontal">`;
            
            // Разная сортировка в зависимости от количества мест
            let sortedSeats;
            if (comp.seats.length === 4) {
                // Для 4 мест: сначала четные (верхние), потом нечетные (нижние)
                sortedSeats = [...comp.seats].sort((a, b) => a % 2 - b % 2 || a - b);
            } else {
                // Для 2 мест: просто по порядку
                sortedSeats = [...comp.seats].sort((a, b) => a - b);
            }
            
            sortedSeats.forEach(seatNum => {
                const isUpper = comp.seats.length === 4 ? seatNum % 2 === 0 : false;
                html += `
                    <div class="seat ${isUpper ? 'upper-seat' : 'lower-seat'}" 
                         data-seat="${seatNum}"
                         data-type="${isUpper ? 'upper' : 'lower'}">
                        ${seatNum}
                    </div>
                `;
            });
            
            html += "</div></div>";
        }
        
        html += "</div>";
    }
    
    // Боковые места (оставляем в линию)
    if (schemaData.schemaType === "Плацкарт" && schemaData.sideSeats && schemaData.sideSeats.length > 0) {
    html += "<div class='side-seats-container-admin'>";
    
    // Сортируем боковые места
    const sortedSideSeats = [...schemaData.sideSeats].sort((a, b) => a - b);
    
    // Группируем по 2 места
    for (let i = 0; i < sortedSideSeats.length; i += 2) {
        const pair = sortedSideSeats.slice(i, i + 2);
        const firstSeatNum = pair[0];
        
        html += `<div class="side-seats-pair" data-first-seat="${firstSeatNum}">`;
        html += `<button class="delete-side-pair-btn" data-first-seat="${firstSeatNum}">×</button>`;
        
        pair.forEach(seatNum => {
            const isUpper = seatNum % 2 === 0;
            html += `
                <div class="side-seat ${isUpper ? 'upper-seat' : 'lower-seat'}" 
                     data-seat="${seatNum}"
                     data-type="${isUpper ? 'upper' : 'lower'}">
                    ${seatNum}
                </div>
            `;
        });
        
        html += "</div>";
    }
    
    html += "</div>";
}
            } else if (schemaData.schemaType === "Сидячий") {
                // Отрисовка сидячего вагона с кнопками управления
                html += "<div id='rowsContainer'>";
                
                for (const row of schemaData.rows) {
                    html += `<div class="row" data-id="${row.id}">`;
                    html += `<div class="row-controls">`;
                    html += `<button class="delete-row-btn" data-id="${row.id}">×</button>`;
                    html += `<button class="move-row-up-btn" data-id="${row.id}">↑</button>`;
                    html += `<button class="move-row-down-btn" data-id="${row.id}">↓</button>`;
                    html += `</div>`;
               
                    html += `<p>Ряд №${row.rowNumber}</p>`;
                    
                    html += "<div class='left-seats'>";
                    for (const seat of row.leftSeats) {
                        html += `<div class="seat">${seat}</div>`;
                    }
                    html += "</div>";
                    
                    html += "<div class='right-seats'>";
                    for (const seat of row.rightSeats) {
                        html += `<div class="seat">${seat}</div>`;
                    }
                    html += "</div>";
                    
                    html += "</div>";
                }
                
                html += "</div>";
            }
            
            visualPreview.innerHTML = html || "<p>Выберите тип вагона и добавьте элементы</p>";
            // Добавляем обработчики для кнопок управления
            setupControls();
        }

        // Настройка обработчиков для кнопок управления
        function setupControls() {
            document.querySelectorAll('.delete-side-pair-btn').forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.stopPropagation();
        const firstSeat = parseInt(e.target.getAttribute('data-first-seat'));
        
        // Удаляем оба места пары (firstSeat и firstSeat+1)
        schemaData.sideSeats = schemaData.sideSeats.filter(seat => 
            seat !== firstSeat && seat !== firstSeat + 1
        );
        updateAllSeatNumbers();
        updateCounters();
        updateVisualPreview();
    });
});
            // Удаление купе
            document.querySelectorAll('.delete-btn').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const id = parseInt(e.target.getAttribute('data-id'));
                    schemaData.compartments = schemaData.compartments.filter(c => c.id !== id);
                    renumberCompartments();
                    updateAllSeatNumbers(); // Обновляем нумерацию всех мест
                    updateCounters();
                    updateVisualPreview();

		    checkVanCompletion();
                });
            });
            
            // Перемещение купе вверх
            document.querySelectorAll('.move-up-btn').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const id = parseInt(e.target.getAttribute('data-id'));
                    moveCompartmentUp(id);
                });
            });
            
            // Перемещение купе вниз
            document.querySelectorAll('.move-down-btn').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const id = parseInt(e.target.getAttribute('data-id'));
                    moveCompartmentDown(id);
                });
            });
            
            // Удаление ряда
            document.querySelectorAll('.delete-row-btn').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const id = parseInt(e.target.getAttribute('data-id'));
                    schemaData.rows = schemaData.rows.filter(r => r.id !== id);
                    renumberRows();
                    updateAllSeatNumbers(); // Обновляем нумерацию всех мест
                    updateVisualPreview();
                });
            });
            
            // Перемещение ряда вверх
            document.querySelectorAll('.move-row-up-btn').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const id = parseInt(e.target.getAttribute('data-id'));
                    moveRowUp(id);
                });
            });
            
            // Перемещение ряда вниз
            document.querySelectorAll('.move-row-down-btn').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const id = parseInt(e.target.getAttribute('data-id'));
                    moveRowDown(id);
                });
            });
        }

        // Перемещение купе вверх
        function moveCompartmentUp(id) {
            const index = schemaData.compartments.findIndex(c => c.id === id);
            if (index <= 0) return;
            
            // Меняем местами с предыдущим
            [schemaData.compartments[index-1], schemaData.compartments[index]] = 
                [schemaData.compartments[index], schemaData.compartments[index-1]];
            
            renumberCompartments();
            updateAllSeatNumbers(); // Обновляем нумерацию всех мест
            updateVisualPreview();
        }

        // Перемещение купе вниз
        function moveCompartmentDown(id) {
            const index = schemaData.compartments.findIndex(c => c.id === id);
            if (index === -1 || index >= schemaData.compartments.length - 1) return;
            
            // Меняем местами со следующим
            [schemaData.compartments[index], schemaData.compartments[index+1]] = 
                [schemaData.compartments[index+1], schemaData.compartments[index]];
            
            renumberCompartments();
            updateAllSeatNumbers(); // Обновляем нумерацию всех мест
            updateVisualPreview();
        }

        // Перемещение ряда вверх
        function moveRowUp(id) {
            const index = schemaData.rows.findIndex(r => r.id === id);
            if (index <= 0) return;
            
            // Меняем местами с предыдущим
            [schemaData.rows[index-1], schemaData.rows[index]] = 
                [schemaData.rows[index], schemaData.rows[index-1]];
            
            renumberRows();
            updateAllSeatNumbers(); // Обновляем нумерацию всех мест
            updateVisualPreview();
        }

        // Перемещение ряда вниз
        function moveRowDown(id) {
            const index = schemaData.rows.findIndex(r => r.id === id);
            if (index === -1 || index >= schemaData.rows.length - 1) return;
            
            // Меняем местами со следующим
            [schemaData.rows[index], schemaData.rows[index+1]] = 
                [schemaData.rows[index+1], schemaData.rows[index]];
            
            renumberRows();
            updateAllSeatNumbers(); // Обновляем нумерацию всех мест
            updateVisualPreview();
        }

        // Перенумерация купе после перемещения/удаления
        function renumberCompartments() {
            schemaData.compartments.forEach((comp, index) => {
                comp.compartmentNumber = index + 1;
            });
        }

        // Перенумерация рядов после перемещения/удаления
        function renumberRows() {
            schemaData.rows.forEach((row, index) => {
                row.rowNumber = index + 1;
            });
        }

        // Обновление нумерации всех мест в правильном порядке
        function updateAllSeatNumbers() {
            let seatCounter = 1;
            
            // Нумеруем места в купе
            for (const comp of schemaData.compartments) {
                for (let i = 0; i < comp.seats.length; i++) {
                    comp.seats[i] = seatCounter++;
                }
            }
            
            // Нумеруем боковые места (только для плацкарта)
            if (schemaData.schemaType === 'Плацкарт') {
		const sideSeatsCount = schemaData.sideSeats.length;
        	schemaData.sideSeats = [];
        
        	// Добавляем новые номера после основных мест
        	for (let i = 0; i < sideSeatsCount; i++) {
            		schemaData.sideSeats.push(seatCounter++);
        	}
            }
            
            // Нумеруем места в сидячем вагоне
            for (const row of schemaData.rows) {
                for (let i = 0; i < row.leftSeats.length; i++) {
                    row.leftSeats[i] = seatCounter++;
                }
                for (let i = 0; i < row.rightSeats.length; i++) {
                    row.rightSeats[i] = seatCounter++;
                }
            }
	    updateCounters();
	    checkVanCompletion();
        }

        // Показать статусное сообщение в конструкторе
        function showConstructorStatus(message, type) {
            constructorStatus.textContent = message;
            constructorStatus.className = `${type}-message`;
            constructorStatus.style.display = 'block';
            
            // Автоматическое скрытие через 3 секунды
            setTimeout(() => {
                constructorStatus.style.display = 'none';
            }, 2000);
        }

        

        // Сброс схемы
        function resetSchema(showConfirm = true) {
            if (showConfirm && !confirm("Вы уверены, что хотите сбросить текущую схему?")) {
                return;
            }
            
            schemaData.schemaName = "";
            schemaData.schemaType = vanTypeSelect.value;
            schemaData.compartments = [];
            schemaData.sideSeats = [];
            schemaData.rows = [];
            
            schemaNameInput.value = "";
            schemaNameContainer.style.display = 'none';
            saveButton.style.display = 'none';
            
            updateCounters();
            updatePreview();
            
            if (showConfirm) {
                showStatus("Схема сброшена", "success");
            }
        }

  // Инициализация загрузки расписания при открытии страницы
    if (window.location.pathname.endsWith('personal-cabinet-admin.html')) {
        setActiveSection('schedule');
        loadSchedule();
    }
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
});