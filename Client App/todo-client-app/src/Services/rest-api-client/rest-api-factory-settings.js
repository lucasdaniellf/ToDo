export function CreateTodoRequestsSettings() {
  
    const settings = {
        baseUrl : `http://localhost:5297/api/ToDo`,
        headers: { 'Content-Type': 'application/json' }
    }

    return settings;
};