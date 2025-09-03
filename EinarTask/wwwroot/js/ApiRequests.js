// Sürükle-Bırak Fonksiyonları
function drag(event) {
    event.dataTransfer.setData("taskId", event.target.getAttribute("data-task-id"));
}

function drop(event) {
    event.preventDefault();
    const taskId = event.dataTransfer.getData("taskId");
    const newTaskTypeId = event.target.closest(".column").getAttribute("data-tasktype-id");

    $.ajax({
        url: '/Tasks/UpdateTaskTypeId',
        type: 'POST',
        data: { taskId: taskId, newTaskTypeId: newTaskTypeId },
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        }
    });
}


// TaskType Adını Güncelle
function updateTaskTypeName(taskTypeId, name) {
    $.ajax({
        url: '/Tasks/UpdateTaskType',
        type: 'POST',
        data: { id: taskTypeId, name: name, color: '' },
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (!response.success) {
                alert(response.message);
            }
        }
    });
}

// TaskType Rengini Güncelle
function updateTaskTypeColor(taskTypeId, color) {
    $.ajax({
        url: '/Tasks/UpdateTaskType',
        type: 'POST',
        data: { id: taskTypeId, name: '', color: color },
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (!response.success) {
                alert(response.message);
            }
        }
    });
}

// TaskType Sil
function deleteTaskType(taskTypeId) {
    if (confirm("Bu görev tipini silmek istediğinize emin misiniz?")) {
        $.ajax({
            url: '/Tasks/DeleteTaskType',
            type: 'POST',
            data: { id: taskTypeId },
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            }
        });
    }
}

// TaskType Düzenle
function editTaskType(taskTypeId) {
    var userId = '@User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)';
    if (!userId) {
        console.error('UserId bulunamadı!');
        return;
    }

    $.ajax({
        url: '/Tasks/CreateOrUpdateTaskType',
        type: 'GET',
        data: { id: taskTypeId, userId: userId },
        beforeSend: function () {
            $('#editTaskTypeModalBody').html('<p>Yükleniyor...</p>');
        },
        success: function (partialView) {
            $('#editTaskTypeModalBody').html(partialView);
            $('#editTaskTypeModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.error('Partial view yüklenirken hata:', error);
            $('#editTaskTypeModalBody').html('<p>Hata oluştu, lütfen tekrar deneyin.</p>');
        }
    });
}
// Modal içindeki form submit edilince güncelleme yap
$(document).on('submit', '#taskTypeForm', function (e) {
    e.preventDefault();

    $.ajax({
        url: '/Tasks/CreateOrUpdateTaskType',
        type: 'POST',
        data: $(this).serialize(),
        success: function (res) {
            if (res.success) {
                $('#createNewTaskTypeModal').modal('hide');
                location.reload();
            } else {
                alert(res.message || "Bir hata oluştu.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Güncelleme sırasında hata:", error);
        }
    });
});

// Görev Sil
function deleteTask(taskId) {
    if (confirm("Bu görevi silmek istediğinize emin misiniz?")) {
        $.ajax({
            url: '/Tasks/DeleteTask',
            type: 'POST',
            data: { id: taskId },
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            }
        });
    }
}

// Modal Açıldığında Partial View Yükle
$('#createTaskModal').on('show.bs.modal', function () {
    var userId = '@User.FindFirstValue(ClaimTypes.NameIdentifier)';
    if (!userId || userId === '') {
        console.error('UserId bulunamadı!');
        $('#createTaskModalBody').html('<p>Kullanıcı kimliği alınamadı, lütfen giriş yapın.</p>');
        return;
    }
    $.ajax({
        url: '/Tasks/CreateTask',
        type: 'GET',
        data: { userId: userId },
        success: function (data) {
            $('#createTaskModalBody').html(data);
        },
        error: function (xhr, status, error) {
            console.error('Partial view yüklenirken hata:', error);
            $('#createTaskModalBody').html('<p>Hata oluştu, lütfen tekrar deneyin. Detaylar konsolda.</p>');
        }
    });
});

// Yeni TaskType Modal Açıldığında
$('#createNewTaskTypeModal').on('show.bs.modal', function () {
    var userId = '@User.FindFirstValue(ClaimTypes.NameIdentifier)';
    if (!userId || userId === '') {
        console.error('UserId bulunamadı!');
        $('#createNewTaskTypeModalBody').html('<p>Kullanıcı kimliği alınamadı, lütfen giriş yapın.</p>');
        return;
    }
    $.ajax({
        url: '/Tasks/CreateOrUpdateTaskType',
        type: 'GET',
        data: { userId: userId },
        success: function (data) {
            $('#createNewTaskTypeModalBody').html(data);
        },
        error: function (xhr, status, error) {
            console.error('Partial view yüklenirken hata:', error);
            $('#createNewTaskTypeModalBody').html('<p>Hata oluştu, lütfen tekrar deneyin. Detaylar konsolda.</p>');
        }
    });
});


// Form Gönderimleri
$(document).on('submit', '#createTaskForm', function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    $.ajax({
        url: '/Tasks/CreateTask',
        type: 'POST',
        data: formData,
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                $('#createTaskModal').modal('hide');
                location.reload();
            } else {
                $('#formErrors').html('<ul>' + response.errors.map(e => '<li>' + e + '</li>').join('') + '</ul>');
            }
        },
        error: function (xhr, status, error) {
            console.log('Hata:', error);
            $('#formErrors').html('<ul><li>Bir hata oluştu, lütfen tekrar deneyin.</li></ul>');
        }
    });
});

$(document).on('submit', '#createTaskTypeForm', function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    $.ajax({
        url: '/Tasks/CreateOrUpdateTaskType',
        type: 'POST',
        data: formData,
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                $('#createNewTaskTypeModal').modal('hide');
                location.reload();
            } else {
                $('#formErrors').html('<ul><li>' + response.message + '</li></ul>');
            }
        },
        error: function (xhr, status, error) {
            console.log('Hata:', error);
            $('#formErrors').html('<ul><li>Bir hata oluştu, lütfen tekrar deneyin.</li></ul>');
        }
    });
});