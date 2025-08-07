package com.secom.emergency.entity;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import java.time.LocalDateTime;

@Entity
@Table(name = "NEW_SIM_REGIST")
public class NewSimRegist {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @NotBlank(message = "電話番号は必須です")
    @Size(min = 11, max = 11, message = "電話番号は11桁で入力してください")
    @Pattern(regexp = "\\d{11}", message = "電話番号は11桁の数字で入力してください")
    @Column(name = "PHONE_NUMBER", length = 11, unique = true, nullable = false)
    private String phoneNumber;
    
    @NotBlank(message = "加入者コードは必須です")
    @Size(min = 8, max = 8, message = "加入者コードは8桁で入力してください")
    @Pattern(regexp = "[A-Za-z0-9]{8}", message = "加入者コードは8桁の英数字で入力してください")
    @Column(name = "SUBSCRIBER_CODE", length = 8, nullable = false)
    private String subscriberCode;
    
    @NotBlank(message = "ICカード番号は必須です")
    @Size(min = 19, max = 19, message = "ICカード番号は19桁で入力してください")
    @Pattern(regexp = "\\d{19}", message = "ICカード番号は19桁の数字で入力してください")
    @Column(name = "IC_CARD_NUMBER", length = 19, unique = true, nullable = false)
    private String icCardNumber;
    
    @NotBlank(message = "ネットワーク暗証番号は必須です")
    @Size(min = 4, max = 4, message = "ネットワーク暗証番号は4桁で入力してください")
    @Pattern(regexp = "\\d{4}", message = "ネットワーク暗証番号は4桁の数字で入力してください")
    @Column(name = "NETWORK_PIN", length = 4, nullable = false)
    private String networkPin;
    
    @NotBlank(message = "SIMロック解除コードは必須です")
    @Size(min = 8, max = 8, message = "SIMロック解除コードは8桁で入力してください")
    @Pattern(regexp = "[A-Za-z0-9]{8}", message = "SIMロック解除コードは8桁の英数字で入力してください")
    @Column(name = "SIM_UNLOCK_CODE", length = 8, nullable = false)
    private String simUnlockCode;
    
    @Column(name = "CREATED_AT", nullable = false)
    private LocalDateTime createdAt;
    
    @Column(name = "UPDATED_AT")
    private LocalDateTime updatedAt;
    
    public NewSimRegist() {
        this.createdAt = LocalDateTime.now();
    }
    
    public NewSimRegist(String phoneNumber, String subscriberCode, String icCardNumber, 
                       String networkPin, String simUnlockCode) {
        this();
        this.phoneNumber = phoneNumber;
        this.subscriberCode = subscriberCode;
        this.icCardNumber = icCardNumber;
        this.networkPin = networkPin;
        this.simUnlockCode = simUnlockCode;
    }
    
    @PreUpdate
    public void preUpdate() {
        this.updatedAt = LocalDateTime.now();
    }
    
    // Getters and Setters
    public Long getId() {
        return id;
    }
    
    public void setId(Long id) {
        this.id = id;
    }
    
    public String getPhoneNumber() {
        return phoneNumber;
    }
    
    public void setPhoneNumber(String phoneNumber) {
        this.phoneNumber = phoneNumber;
    }
    
    public String getSubscriberCode() {
        return subscriberCode;
    }
    
    public void setSubscriberCode(String subscriberCode) {
        this.subscriberCode = subscriberCode;
    }
    
    public String getIcCardNumber() {
        return icCardNumber;
    }
    
    public void setIcCardNumber(String icCardNumber) {
        this.icCardNumber = icCardNumber;
    }
    
    public String getNetworkPin() {
        return networkPin;
    }
    
    public void setNetworkPin(String networkPin) {
        this.networkPin = networkPin;
    }
    
    public String getSimUnlockCode() {
        return simUnlockCode;
    }
    
    public void setSimUnlockCode(String simUnlockCode) {
        this.simUnlockCode = simUnlockCode;
    }
    
    public LocalDateTime getCreatedAt() {
        return createdAt;
    }
    
    public void setCreatedAt(LocalDateTime createdAt) {
        this.createdAt = createdAt;
    }
    
    public LocalDateTime getUpdatedAt() {
        return updatedAt;
    }
    
    public void setUpdatedAt(LocalDateTime updatedAt) {
        this.updatedAt = updatedAt;
    }
}