import { useState, useEffect } from 'react';
import { currentUser, setCurrentUser } from 'react';
import { environment } from '../environments/environment.development';

export class AccountService {
    baseUrl: string;
    constructor()
    {
        this.baseUrl = environment.apiUrl;
        useEffect(() => {
            const storedUser = JSON.parse(localStorage.getItem('user'));
            if (storedUser) {
                setCurrentUser(storedUser);
            }
        });
    }

    async login(model) {
        try {
            const response = await fetch(this.baseUrl + 'account/login', {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json'
                },
                body: JSON.stringify(model)
            });
            const user = await response.json();
            return user;
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    }

}