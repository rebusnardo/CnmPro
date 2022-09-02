import axios from 'axios';
import { API_HOST_PREFIX, onGlobalSuccess, onGlobalError } from './serviceHelpers';
const endpoint = `${API_HOST_PREFIX}/api/messages`;

export const getCurrentUser = () => {
    const config = {
        method: 'GET',
        url: 'https://localhost:50001/api/user/current',
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const getSenderById = (id, pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endpoint}/sender/${id}/?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const getReceiverById = (id, pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endpoint}/receiver/${id}/?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const getByConversation = (id, pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endpoint}/conversation/${id}/?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const createNewMessage = (payload) => {
    const config = {
        method: 'POST',
        url: endpoint,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const deleteMessageById = (id) => {
    const config = {
        method: 'DELETE',
        url: `${endpoint}/ ${id}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};
const messageService = {
    getSenderById,
    getReceiverById,
    getCurrentUser,
    createNewMessage,
    deleteMessageById,
    getByConversation,
};

export default messageService;
