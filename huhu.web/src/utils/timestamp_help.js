/**
 * 10位补全为13位
 * @param {*} timestamp 
 * @returns 
 */
const bit10_to_bit13 = (timestamp) => {
    let result = (timestamp + '').split('');
    for (let start = 0; start < 13; start++) {
        if (!result[start]) {
            result[start] = '0';
        }
    }
    return result;
}

/**
 * 发布几分钟前
 * @param {*} timestamp 
 * @returns 
 */
const otherday = (timestamp) => {
    timestamp = bit10_to_bit13(timestamp).join('') * 1;
    let minute = 1000 * 60;
    let hour = minute * 60;
    let day = hour * 24;
    let month = day * 30;
    let now = new Date().getTime();
    let diffValue = now - timestamp;
    //如果本地时间反而小于变量时间
    if (diffValue < 0) {
        return '不久前';
    }
    //计算差异时间的量级
    let monthC = diffValue / month;
    let weekC = diffValue / (7 * day);
    let dayC = diffValue / day;
    let hourC = diffValue / hour;
    let minC = diffValue / minute;
    //数值补0
    let zero = function (value) {
        if (value < 10) {
            return '0' + value;
        }
        return value;
    };
    if (monthC > 4) {
        //超过1年，直接显示年月日
        return (() => {
            let date = new Date(timestamp);
            return date.getFullYear() + '年' + zero(date.getMonth() + 1) + '月' + zero(date.getDate()) + '日';
        })();
    } else if (monthC >= 1) {
        return parseInt(monthC) + "月前";
    } else if (weekC >= 1) {
        return parseInt(weekC) + "周前";
    } else if (dayC >= 1) {
        return parseInt(dayC) + "天前";
    } else if (hourC >= 1) {
        return parseInt(hourC) + "小时前";
    } else if (minC >= 1) {
        return parseInt(minC) + "分钟前";
    }
    return '刚刚';
}

//年月日
const yeardate = (format, timestamp) => {
    timestamp = bit10_to_bit13(timestamp).join('') * 1;
    let time = new Date(parseInt(timestamp));
    let year = time.getFullYear();
    let month = time.getMonth() + 1;
    let day = time.getDate();
    let hour = time.getHours();
    let minute = time.getMinutes();
    let second = time.getSeconds();

    month = month < 10 ? ('0' + month) : month;
    day = day < 10 ? ('0' + day) : day;
    hour = hour < 10 ? ('0' + hour) : hour;
    minute = minute < 10 ? ('0' + minute) : minute;
    second = second < 10 ? ('0' + second) : second;

    switch (true) {
        case (format === 'YYYY-mm-dd'):
            return year + '-' + month + '-' + day;
        case (format === 'YYYY年mm月dd日'):
            return year + '年' + month + '月' + day + '日';
        case (format === 'YYYY年mm月dd日 HH:MM'):
            return year + '年' + month + '月' + day + '日 ' + hour + ':' + minute;
        case (format === 'YYYY-mm-dd HH:MM'):
            return year + '-' + month + '-' + day + ' ' + hour + ':' + minute;
        case (format === 'YYYY-mm-dd HH:MM:SS'):
            return year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second;
        default: return timestamp;
    }
}


const timestamp_help = () => {
    return {
        otherday,
        yeardate,
    }
}
export default timestamp_help;