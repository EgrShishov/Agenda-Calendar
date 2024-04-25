function handleCheckboxClick(calendarId) {
    var checkbox = document.getElementById('calendar-' + calendarId);
    var events = calendar.getEvents();
    events.forEach(function (event) {
        if (event.extendedProps.calendarId == calendarId) {
            if (checkbox.checked) {
                event.setProp('display', 'auto');
            }
            else {
                event.setProp('display', 'none');
            }
        }
    });
}

function closeEventCardInformation() {
    document.getElementById('event-card-close').addEventListener('click', function () {
        document.getElementById('event-details-modal').display = 'none';
    });
}
function showEventCardInformation(info) {
    var card = document.getElementById('event-details-modal');
    card.style.display = 'block';
    card.setAttribute('data-id', info.id);
    document.getElementById('event-startTime').innerText = info.start;
    document.getElementById('event-endTime').innerText = info.end;
    document.getElementById('event-title').innerText = info.title;
    document.getElementById('event-description').innerText = info.extendedProps.description;

    document.getElementById('edit-event-btn').addEventListener('click', async function () {
        let id = card.attributes.getNamedItem('data-id');
        console.log(id)
        let response = await fetch('/Event/Edit?id=' + id.value);
        if (response.ok) {
            let html = await response.text();
            let parser = new DOMParser();
            let newDocument = parser.parseFromString(html, 'text/html');
            document.documentElement.replaceWith(newDocument.documentElement);
            //window.history.pushState({}, '', '/Event/Edit?id=' + id.value);
            console.log(html);
        } else {
            alert('Error' + response.status);
        }
    });

    document.getElementById('delete-event-btn').addEventListener('click', async function () {
        let id = card.attributes.getNamedItem('data-id');
        let request = await fetch('/Event/Delete?id=' + id.value, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
        });
        if (request.ok) {
            var event = calendar.getEventById(id.value);
            event.remove();
            card.style.display = 'none';
        } else {
            alert('There are some errors: ' + request.status);
        }
    });

    document.getElementById('event-card-close').addEventListener('click', function () {
        card.style.display = 'none';
    });

    window.onclick = function (event) {

    };
}

async function eventChangeHandler(changeinfo) {
    var updatedEvent = changeinfo.event;
    var oldEvent = changeinfo.oldEvent;

    if (oldEvent.start != updatedEvent.start || oldEvent.end != updatedEvent.end) {
        let request = await fetch('/Event/Edit?id=' + updatedEvent.id, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify(updatedEvent)
        });
        console.log(request);
    }
}