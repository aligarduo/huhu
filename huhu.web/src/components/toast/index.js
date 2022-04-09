import React from 'react'
import ReactDOM from 'react-dom'
import Toast from './toast'
import './toast.css'

function createNotification() {
    const div = document.getElementsByClassName("main-alert-list")[0]
    const notification = ReactDOM.render(<Toast />, div)
    return {
        addNotice(notice) {
            return notification.addNotice(notice)
        },
        destroy() {
            ReactDOM.unmountComponentAtNode(div)
            document.body.removeChild(div)
        }
    }
}

let notification
const notice = (type, content, duration = 2000, onClose) => {
    if (!notification) notification = createNotification()
    return notification.addNotice({ type, content, duration, onClose })
}

const toast = {
    info(content, duration, onClose) {
        return notice('info', content, duration, onClose)
    },
    success(content = 'okok', duration, onClose) {
        return notice('success', content, duration, onClose)
    },
    error(content, duration, onClose) {
        return notice('error', content, duration, onClose)
    },
    loading(content = 'loading...', duration = 0, onClose) {
        return notice('loading', content, duration, onClose)
    }
}
export default toast;