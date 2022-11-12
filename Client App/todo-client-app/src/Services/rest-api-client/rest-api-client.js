export class RestApiClient{
    constructor(_settings){
        this.settings = _settings;
    }

    async getTodos(signal) {
        let url = this.settings.baseUrl;
        let response = await fetch(url, {signal: signal});
        return response;
    }

    async getTodoById(id,signal){

        let url = `${this.settings.baseUrl}/${id}`;
        let response = await fetch(url, {signal: signal});
        
        return response;
    }

    async updateTodo(id, obj){

        let url = `${this.settings.baseUrl}/${id}`;
        
        const requestOptions = {
            method: 'PUT',
            headers: this.settings.headers,
            body: JSON.stringify(obj)
        };

        let response = await fetch(url, requestOptions);
        return response;
    }

    async deleteTodo(id){
        let url = `${this.settings.baseUrl}/${id}`;

        const requestOptions = {
            method: 'DELETE',
            headers: this.settings.headers,
        };

        let response = await fetch(url, requestOptions);
        return response;
    }

    async postTodo(obj){
        let url = `${this.settings.baseUrl}`;

        const requestOptions = {
            method: 'POST',
            headers: this.settings.headers,
            body: JSON.stringify(obj)
        };

        let response = await fetch(url, requestOptions);
        return response;
    }

    async updateTodoStatus(id, obj){
        let url = `${this.settings.baseUrl}/${id}/status`;

        const requestOptions = {
            method: 'PUT',
            headers: this.settings.headers,
            body: JSON.stringify(obj)
        };

        let response = await fetch(url, requestOptions);
        return response;
    }
}