#!/bin/bash

path="./env"

source $path

chatId="-4118825976"
botToken="6967461037:AAFrrjvMvLbhJrv6tfeACLlUM08pFwIkauc"
productionLink="https://backend.streetcode.com.ua"
oldToken=""
newToken=""
massage=""
massageId=""

set_new_token() {
    awk -v newToken="$newToken" '/^INSTAGRAM_TOKEN=/{gsub(/=.*/, "=\"" newToken "\"")}1' "$path" > tmpfile && mv tmpfile "$path"
}

check_token_changing() {
    echo "$(grep "INSTAGRAM_TOKEN" "$path" | cut -d '=' -f 2)"
    echo $oldToken
    echo "$oldToken"
    if [ $(grep "INSTAGRAM_TOKEN" "$path" | cut -d '=' -f 2) -ne "$oldToken" ]; then
        return 0 
    else
        return 1
    fi
}

refresh_token() {
    local response=$(curl "https://graph.instagram.com/refresh_access_token?grant_type=ig_refresh_token&&access_token=$oldToken")
    echo "$response"
}

send_message_and_get_message_id() {
    local response=$(curl POST "https://api.telegram.org/bot$botToken/sendMessage" \
                    -d "chat_id=$chatId" \
                    -d "text=$massage" \
                    -d "disable_notification=true")
    echo "$response" 
}

update_message() {
    local addition="$1"
    massage+=$addition
    curl POST "https://api.telegram.org/bot$botToken/editMessageText" \
         -d "chat_id=$chatId" \
         -d "message_id=$massageId" \
         -d "text=$massage"
}

main() {
    massage+=$'🎦 Updating Instagram Token'

    massageId=$(send_message_and_get_message_id | jq -r '.result.message_id')

    oldToken="$INSTAGRAM_TOKEN"

    checkTokenValidationResponse=$(curl --write-out '%{http_code}' --silent --output /dev/null "https://graph.instagram.com/me?access_token=$oldToken")

    if [ $checkTokenValidationResponse -ne "200" ]; then

        update_message $'\n❌ Old token is invalid or expired'
    else
        update_message $'\n✅ Old token is valid'

        refreshTokenResponse=$(refresh_token)

        if [ ! $refreshTokenResponse =~ "success" ]; then
            update_message $'\n❌ Token refreshment request failed \n'
            update_message "$refreshTokenResponse"
        else
            update_message $'\n✅ Token was successfully refreshed'
            
            newToken=$(echo "$refreshTokenResponse" | jq -r '.access_token')
            checkTokenValidationResponseNewToken=$(curl --write-out '%{http_code}' --silent --output /dev/null "https://graph.instagram.com/me?access_token=$newToken")

            if [ $checkTokenValidationResponseNewToken -ne "200" ]; then
                update_message $'\n❌ New token is invalid or expired \n'
                update_message "$checkTokenValidationResponseNewToken"
            else
                update_message $'\n✅ New token is valid'

                #set_new_token
                source $path

                check_token_changing

                if [ $? -ne 0 ]; then
                    update_message $'\n❌ Token wasn\'t changed'
                else
                    
                    update_message $'\n✅ Token was successfully changed'

                    update_message $'\n🔁 Reload Virtual machine'
                    # curl -X POST -u dev:x0MFunOszY6YxeDmAl2ZMlpY4Xh http://138.91.120.146:8080/restart
                    
                    check_responce_on_stage_responce=$(curl --write-out '%{http_code}' --silent --output /dev/null "$ENV_API_BACKEND/instagram/getAll")
                    if [ "$check_responce_on_stage_responce" -ne "200" ]; then
                        update_message $'\n❌ Instagram request doesn`t work on stage\n'
                        update_message "$check_responce_on_stage_responce"
                    else
                        update_message $'\n✅ Instagram request works on stage'
                    fi

                    check_responce_on_production_responce=$(curl --write-out '%{http_code}' --silent --output /dev/null "$productionLink/api/instagram/getAll")
                    if [ "$check_responce_on_production_responce" -ne "200" ]; then
                        update_message $'\n❌ Instagram request doesn`t work on prod\n'
                        update_message "$check_responce_on_production_responce"
                    else
                        update_message $'\n✅ Instagram request works on prod'
                    fi
                    
                fi
                
            fi
        fi
    fi
}

main