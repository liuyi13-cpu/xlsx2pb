// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: keyword.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from keyword.proto</summary>
public static partial class KeywordReflection {

  #region Descriptor
  /// <summary>File descriptor for keyword.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static KeywordReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "Cg1rZXl3b3JkLnByb3RvKrEGCghBc3NldFJlZhIVChFBU1NFVF9SRUZfSU5W",
          "QUxJRBAAEhIKDkFTU0VUX1JFRl9JTklUEAESEAoMQVNTRVRfUkVGX0dNEAIS",
          "HAoYQVNTRVRfUkVGX0hPTUVXT1JMRF9GSVNIEAoSHwobQVNTRVRfUkVGX0hP",
          "TUVXT1JMRF9GTE9XRVIxEAsSHwobQVNTRVRfUkVGX0hPTUVXT1JMRF9GTE9X",
          "RVIyEAwSHwobQVNTRVRfUkVGX0hPTUVXT1JMRF9GTE9XRVIzEA0SHwobQVNT",
          "RVRfUkVGX0hPTUVXT1JMRF9GTE9XRVI0EA4SHQoZQVNTRVRfUkVGX0hPTUVX",
          "T1JMRF9TQUxFMRAPEh0KGUFTU0VUX1JFRl9IT01FV09STERfU0FMRTIQEBId",
          "ChlBU1NFVF9SRUZfSE9NRVdPUkxEX1NBTEUzEBESJgoiQVNTRVRfUkVGX0hP",
          "TUVXT1JMRF9DQVRfU0FMRV9SRUFEWRASEiAKHEFTU0VUX1JFRl9IT01FV09S",
          "TERfQ0FUX1NBTEUQExIiCh5BU1NFVF9SRUZfSE9NRVdPUkxEX1NBTEVfUElD",
          "SzEQFBIiCh5BU1NFVF9SRUZfSE9NRVdPUkxEX1NBTEVfUElDSzIQFRIiCh5B",
          "U1NFVF9SRUZfSE9NRVdPUkxEX1NBTEVfUElDSzMQFhIfChtBU1NFVF9SRUZf",
          "SE9NRVdPUkxEX1VOU0hPV04QFxIXChNBU1NFVF9SRUZfQlVZX0NMRVJLEGQS",
          "IQodQVNTRVRfUkVGX0FDSF9SRVdBUkRfQ0xFUktfU1AQZRIhCh1BU1NFVF9S",
          "RUZfQUNIX1JFV0FSRF9DVVJSRU5DWRBmEhUKEUFTU0VUX1JFRl9WSVBHQU1F",
          "EGcSHAoYQVNTRVRfUkVGX1NUT1JFX0RJQU1PTkRTEGgSJgoiQVNTRVRfUkVG",
          "X1NUT1JFX0ZSRUVfR0lGVF9DTEVSS19TUBBpEiYKIkFTU0VUX1JFRl9TVE9S",
          "RV9GUkVFX0dJRlRfQ1VSUkVOQ1kQahIWChJBU1NFVF9SRUZfR0lGVENPREUQ",
          "axIWChJBU1NFVF9SRUZfUk9VTEVUVEUQbCpBCg1FcnJvckNvZGVUeXBlEhkK",
          "FUVycm9yQ29kZVR5cGVfSW52YWxpZBAAEhUKEUVycm9yQ29kZVR5cGVfVGlw",
          "EAEqdgoNUmV3YXJkR2V0VHlwZRIZChVSZXdhcmRHZXRUeXBlX0ludmFsaWQQ",
          "ABIYChRSZXdhcmRHZXRUeXBlX05vcm1hbBABEhgKFFJld2FyZEdldFR5cGVf",
          "RG91YmxlEAISFgoSUmV3YXJkR2V0VHlwZV9IYWxmEAMqrQEKB05wY1R5cGUS",
          "EwoPTnBjVHlwZV9JbnZhbGlkEAASEAoMTnBjVHlwZV9IZXJvEAESFQoRTnBj",
          "VHlwZV9Gcm9udERlc2sQAhIWChJOcGNUeXBlX0hlcm9GYXRoZXIQAxIWChJO",
          "cGNUeXBlX0hlcm9Nb3RoZXIQBBIPCgtOcGNUeXBlX0NhdBAFEg8KC05wY1R5",
          "cGVfRG9nEAYSEgoOTnBjVHlwZV9BbHBhY2EQByqpAQoSSG9tZXdvcmxkUGV0",
          "U3RhdHVzEh4KGkhvbWV3b3JsZFBldFN0YXR1c19JbnZhbGlkEAASGwoXSG9t",
          "ZXdvcmxkUGV0U3RhdHVzX0lkbGUQARIcChhIb21ld29ybGRQZXRTdGF0dXNf",
          "R29PdXQQAhIdChlIb21ld29ybGRQZXRTdGF0dXNfR29Ib21lEAMSGQoVSG9t",
          "ZXdvcmxkUGV0U3RhdHVzX0NEEAQqXwoPSG9tZXdvcmxkU3RhdHVzEhgKFEhv",
          "bWV3b3JsZFN0YXR1c19MT0NLEAASFgoSSG9tZXdvcmxkU3RhdHVzX0NEEAES",
          "GgoWSG9tZXdvcmxkU3RhdHVzX1VOTE9DSxACKp4BChVIb21ld29ybGRGbG93",
          "ZXJTdGF0dXMSIQodSG9tZXdvcmxkRmxvd2VyU3RhdHVzX0ludmFsaWQQABIf",
          "ChtIb21ld29ybGRGbG93ZXJTdGF0dXNfU1RBUlQQARIhCh1Ib21ld29ybGRG",
          "bG93ZXJTdGF0dXNfV09SS0lORxACEh4KGkhvbWV3b3JsZEZsb3dlclN0YXR1",
          "c19ET05FEAMqrQEKGEhvbWV3b3JsZEZsb3dlcmJlZFN0YXR1cxIkCiBIb21l",
          "d29ybGRGbG93ZXJiZWRTdGF0dXNfSW52YWxpZBAAEiIKHkhvbWV3b3JsZEZs",
          "b3dlcmJlZFN0YXR1c19FTVBUWRABEiQKIEhvbWV3b3JsZEZsb3dlcmJlZFN0",
          "YXR1c19XT1JLSU5HEAISIQodSG9tZXdvcmxkRmxvd2VyYmVkU3RhdHVzX0RP",
          "TkUQAypoChFIb21ld29ybGRGaXNoVHlwZRIdChlIb21ld29ybGRGaXNoVHlw",
          "ZV9JTlZBTElEEAASGgoWSG9tZXdvcmxkRmlzaFR5cGVfRlJFRRABEhgKFEhv",
          "bWV3b3JsZEZpc2hUeXBlX0FEEAIqdgoUSG9tZXdvcmxkQXNzZXRTdGF0dXMS",
          "HQoZSG9tZXdvcmxkQXNzZXRTdGF0dXNfTG9jaxAAEh8KG0hvbWV3b3JsZEFz",
          "c2V0U3RhdHVzX1VubG9jaxABEh4KGkhvbWV3b3JsZEFzc2V0U3RhdHVzX1Vz",
          "aW5nEAIq3gEKF0hvbWV3b3JsZEFzc2V0Q29uZGl0aW9uEiMKH0hvbWV3b3Js",
          "ZEFzc2V0Q29uZGl0aW9uX0RlZmF1bHQQABImCiJIb21ld29ybGRBc3NldENv",
          "bmRpdGlvbl9Db2luVW5sb2NrEAESJwojSG9tZXdvcmxkQXNzZXRDb25kaXRp",
          "b25fR3JhZGVVbmxvY2sQAhInCiNIb21ld29ybGRBc3NldENvbmRpdGlvbl9M",
          "b2dpblVubG9jaxADEiQKIEhvbWV3b3JsZEFzc2V0Q29uZGl0aW9uX0FkVW5s",
          "b2NrEAQqiwIKFkhvbWV3b3JsZEFzc2V0QnVmZlR5cGUSEAoMSEFCVF9EZWZh",
          "dWx0EAASFgoSSEFCVF9GbG93ZXJQcmljZVVwEAESFAoQSEFCVF9GaXNoUHJp",
          "Y2VVcBACEhgKFEhBQlRfRmxvd2VyUHJvZHVjZVVwEAMSFgoSSEFCVF9GaXNo",
          "UHJvZHVjZVVwEAQSGQoVSEFCVF9GbG93ZXJSYW5kb21Nb3JlEAUSFwoTSEFC",
          "VF9GaXNoUmFuZG9tTW9yZRAGEhgKFEhBQlRfUGV0TGVzc1NlbGxUaW1lEAcS",
          "GAoUSEFCVF9QZXRMZXNzUmVzdFRpbWUQCBIXChNIQUJUX1BldE1vcmVTZWxs",
          "RXhwEAkqYgoNSW5mb0JveFN0YXR1cxIYChRJbmZvQm94U3RhdHVzX0xvY2tl",
          "ZBAAEhsKF0luZm9Cb3hTdGF0dXNfVW5Mb2NraW5nEAESGgoWSW5mb0JveFN0",
          "YXR1c19VbkxvY2tlZBACKnsKDkluZm9UYXNrU3RhdHVzEhoKFkluZm9UYXNr",
          "U3RhdHVzX1VuU3RhcnQQABIYChRJbmZvVGFza1N0YXR1c19Eb2luZxABEhcK",
          "E0luZm9UYXNrU3RhdHVzX0RvbmUQAhIaChZJbmZvVGFza1N0YXR1c19GYWls",
          "dXJlEAMqlgEKE0luZm9EYWlseVRhc2tTdGF0dXMSHwobSW5mb0RhaWx5VGFz",
          "a1N0YXR1c19VblN0YXJ0EAASHQoZSW5mb0RhaWx5VGFza1N0YXR1c19Eb2lu",
          "ZxABEh4KGkluZm9EYWlseVRhc2tTdGF0dXNfUmV3YXJkEAISHwobSW5mb0Rh",
          "aWx5VGFza1N0YXR1c19Ob1RpbWVzEAMqsgEKDkluZm9TaG9wU3RhdHVzEhcK",
          "E0luZm9TaG9wU3RhdHVzX0xvY2sQABIZChVJbmZvU2hvcFN0YXR1c19Vbmxv",
          "Y2sQARIbChdJbmZvU2hvcFN0YXR1c19CdWlsZGluZxACEhYKEkluZm9TaG9w",
          "U3RhdHVzX0ZpdBADEhgKFEluZm9TaG9wU3RhdHVzX0Nsb3NlEAQSHQoZSW5m",
          "b1Nob3BTdGF0dXNfRGVjb3JhdGluZxAFKrIBChJJbmZvRGVjb3JhdGVTdGF0",
          "dXMSGwoXSW5mb0RlY29yYXRlU3RhdHVzX0xvY2sQABIdChlJbmZvRGVjb3Jh",
          "dGVTdGF0dXNfVW5sb2NrEAESHQoZSW5mb0RlY29yYXRlU3RhdHVzX1JlcGFp",
          "chACEh8KG0luZm9EZWNvcmF0ZVN0YXR1c19FcXVpcHBlZBADEiAKHEluZm9E",
          "ZWNvcmF0ZVN0YXR1c19SZXBhaXJpbmcQBCqEAQoQSW5mb0RlY29yYXRlVHlw",
          "ZRIbChdJbmZvRGVjb3JhdGVUeXBlX05vcm1hbBAAEhkKFUluZm9EZWNvcmF0",
          "ZVR5cGVfRmFzdBABEhsKF0luZm9EZWNvcmF0ZVR5cGVfQWRGYXN0EAISGwoX",
          "SW5mb0RlY29yYXRlVHlwZV9GaW5pc2gQAyp6Cg5JbmZvR29vZFN0YXR1cxIa",
          "ChZJbmZvR29vZFN0YXR1c19JbnZhbGlkEAASFwoTSW5mb0dvb2RTdGF0dXNf",
          "V2FpdBABEhoKFkluZm9Hb29kU3RhdHVzX1dvcmtpbmcQAhIXChNJbmZvR29v",
          "ZFN0YXR1c19Eb25lEAMqNgoISUFTdGF0dXMSEwoPSUFTdGF0dXNfTG9ja2Vk",
          "EAASFQoRSUFTdGF0dXNfVW5Mb2NrZWQQASpWCgxJQVNJdGVtdGF0dXMSFwoT",
          "SUFTSXRlbXRhdHVzX1Jld2FyZBAAEhYKEklBU0l0ZW10YXR1c19Eb2luZxAB",
          "EhUKEUlBU0l0ZW10YXR1c19Eb25lEAIq3QEKEklBU1N0b3JlSXRlbVN0YXR1",
          "cxIgChxJQVNTdG9yZUl0ZW1TdGF0dXNfRXhjaGFuZ2VkEAASIAocSUFTU3Rv",
          "cmVJdGVtU3RhdHVzX05vdEVub3VnaBABEiEKHUlBU1N0b3JlSXRlbVN0YXR1",
          "c19FeGNoYW5nZUNEEAISIgoeSUFTU3RvcmVJdGVtU3RhdHVzX0NhbkV4Y2hh",
          "bmdlEAMSHQoZSUFTU3RvcmVJdGVtU3RhdHVzX0NhblRyeRAEEh0KGUlBU1N0",
          "b3JlSXRlbVN0YXR1c19UcnlpbmcQBSqUCAoQSW5mb0FjaENvbmRpdGlvbhIZ",
          "ChVJbmZvQWNoQ29uZGl0aW9uX05vbmUQABIZChVJbmZvQWNoQ29uZGl0aW9u",
          "X1NhbGUQARIcChhJbmZvQWNoQ29uZGl0aW9uX1Byb2R1Y2UQAhIbChdJbmZv",
          "QWNoQ29uZGl0aW9uX1NhdmluZxADEhsKF0luZm9BY2hDb25kaXRpb25fSW5j",
          "b21lEAQSIQodSW5mb0FjaENvbmRpdGlvbl9PbmxpbmVJbmNvbWUQBRIaChZJ",
          "bmZvQWNoQ29uZGl0aW9uX0xldmVsEAYSHgoaSW5mb0FjaENvbmRpdGlvbl9T",
          "aG9wQ291bnQQBxIbChdJbmZvQWNoQ29uZGl0aW9uX1Nob3BMdhAIEiQKIElu",
          "Zm9BY2hDb25kaXRpb25fTm9ybWFsVGFza0NvdW50EAkSIwofSW5mb0FjaENv",
          "bmRpdGlvbl9MaW1pdFRhc2tDb3VudBAKEh8KG0luZm9BY2hDb25kaXRpb25f",
          "Q29zdEVuZXJneRALEh0KGUluZm9BY2hDb25kaXRpb25fTmV3R3Vlc3QQDBIf",
          "ChtJbmZvQWNoQ29uZGl0aW9uX05ld1NlcnZpY2UQDRIdChlJbmZvQWNoQ29u",
          "ZGl0aW9uX0JveENvdW50EA4SHAoYSW5mb0FjaENvbmRpdGlvbl9TUENvdW50",
          "EA8SHAoYSW5mb0FjaENvbmRpdGlvbl9DbGVya0x2EBASHAoYSW5mb0FjaENv",
          "bmRpdGlvbl9BRENvdW50EBESIQodSW5mb0FjaENvbmRpdGlvbl9DbGVya0x2",
          "Q291bnQQEhIfChtJbmZvQWNoQ29uZGl0aW9uX0xvZ2luQ291bnQQExIZChVJ",
          "bmZvQWNoQ29uZGl0aW9uX0Zpc2gQFBIbChdJbmZvQWNoQ29uZGl0aW9uX0Zs",
          "b3dlchAVEh8KG0luZm9BY2hDb25kaXRpb25fRmlzaEluY29tZRAWEiEKHUlu",
          "Zm9BY2hDb25kaXRpb25fRmxvd2VySW5jb21lEBcSGAoUSW5mb0FjaENvbmRp",
          "dGlvbl9QZXQQGBIaChZJbmZvQWNoQ29uZGl0aW9uX0hvdXNlEBkSGAoUSW5m",
          "b0FjaENvbmRpdGlvbl9DYXIQGhIeChpJbmZvQWNoQ29uZGl0aW9uX0Zpc2hM",
          "ZXZlbBAbEiAKHEluZm9BY2hDb25kaXRpb25fRmxvd2VyTGV2ZWwQHBIZChVJ",
          "bmZvQWNoQ29uZGl0aW9uX0dhbWUQHRIgChxJbmZvQWNoQ29uZGl0aW9uX0dh",
          "bWVQZXJmZWN0EB4SHgoaSW5mb0FjaENvbmRpdGlvbl9TaG9wU2tpbGwQHxIi",
          "Ch5JbmZvQWNoQ29uZGl0aW9uX1NjZW5lRGVjb3JhdGUQICqgAQoISW5mb0J1",
          "ZmYSFwoTSW5mb0J1ZmZfTmV3YmllVGFzaxAAEhcKE0luZm9CdWZmX0NsZXJr",
          "U2tpbGwQARIWChJJbmZvQnVmZl9TaG9wU2tpbGwQAhIQCgxJbmZvQnVmZl9B",
          "Y2gQAxIPCgtJbmZvQnVmZl9BZBAEEhAKDEluZm9CdWZmX1BldBAFEhUKEUlu",
          "Zm9CdWZmX0RlY29yYXRlEAYqjwQKDEluZm9CdWZmVHlwZRIVChFJbmZvQnVm",
          "ZlR5cGVfTm9uZRAAEhoKFkluZm9CdWZmVHlwZV9FbmVyZ3lNYXgQAxIZChVJ",
          "bmZvQnVmZlR5cGVfR3Vlc3RNYXgQBBIaChZJbmZvQnVmZlR5cGVfQXV0b0dv",
          "b2RzEAYSHAoYSW5mb0J1ZmZUeXBlX0VuZXJneVNwZWVkEAcSHQoZSW5mb0J1",
          "ZmZUeXBlX1Byb2R1Y2VTcGVlZBAIEhgKFEluZm9CdWZmVHlwZV9QcmljZVVw",
          "EAkSGwoXSW5mb0J1ZmZUeXBlX1Rhc2tUaW1lVXAQChIaChZJbmZvQnVmZlR5",
          "cGVfVGFza0V4cFVwEAsSGQoVSW5mb0J1ZmZUeXBlX0JveFNwZWVkEAwSHwob",
          "SW5mb0J1ZmZUeXBlX0d1ZXN0TW92ZVNwZWVkEA0SGQoVSW5mb0J1ZmZUeXBl",
          "X0J1eVNwZWVkEA4SHwobSW5mb0J1ZmZUeXBlX09ubGluZVJld2FyZFVwEA8S",
          "HQoZSW5mb0J1ZmZUeXBlX0d1ZXN0Q29pbnNVcBAQEhgKFEluZm9CdWZmVHlw",
          "ZV9TYWxlRXhwEBESHAoYSW5mb0J1ZmZUeXBlX0ZyZWVQcm9kdWNlEBISGwoX",
          "SW5mb0J1ZmZUeXBlX0V4dHJhR29vZHMQExIZChVJbmZvQnVmZlR5cGVfQWRk",
          "R29vZHMQFCpMCglTa2lsbFR5cGUSEgoOU2tpbGxUeXBlX05vbmUQABIVChFT",
          "a2lsbFR5cGVfUGFzc2l2ZRABEhQKEFNraWxsVHlwZV9BY3RpdmUQAiq2AgoP",
          "U2tpbGxFZmZlY3RUeXBlEhgKFFNraWxsRWZmZWN0VHlwZV9Ob25lEAASHwob",
          "U2tpbGxFZmZlY3RUeXBlX0FkZEdvb2RzTnVtEAESIAocU2tpbGxFZmZlY3RU",
          "eXBlX0FkZEF1dG9Db3VudBACEiMKH1NraWxsRWZmZWN0VHlwZV9BZGRQcm9k",
          "dWNlU3BlZWQQAxInCiNTa2lsbEVmZmVjdFR5cGVfQWRkR29vZHNQcm9iYWJp",
          "bGl0eRAEEh4KGlNraWxsRWZmZWN0VHlwZV9BZGRTYWxlRXhwEAUSHAoYU2tp",
          "bGxFZmZlY3RUeXBlX0FkZEdvb2RzEAYSHAoYU2tpbGxFZmZlY3RUeXBlX0Fk",
          "ZFNwZWVkEAcSHAoYU2tpbGxFZmZlY3RUeXBlX0FkZENvaW5zEAgqXgoNSW5m",
          "b1RpbWVzRGF0YRIWChJJbmZvVGltZXNEYXRhX05vbmUQABIZChVJbmZvVGlt",
          "ZXNEYXRhX09wZW5Cb3gQARIaChZJbmZvVGltZXNEYXRhX0V4dHJhQm94EAIq",
          "cgoTU2hvcFNraWxsRWZmZWN0VHlwZRIfChtTaG9wU2tpbGxFZmZlY3RUeXBl",
          "X0ludmFsaWQQABIcChhTaG9wU2tpbGxFZmZlY3RUeXBlX05aREMQARIcChhT",
          "aG9wU2tpbGxFZmZlY3RUeXBlX1RIU1EQAiqiBAoSSW5mb05ld2JpZVRhc2tU",
          "eXBlEhsKF0luZm9OZXdiaWVUYXNrVHlwZV9Ob25lEAASIQodSW5mb05ld2Jp",
          "ZVRhc2tUeXBlX1VubG9ja1Nob3AQARIiCh5JbmZvTmV3YmllVGFza1R5cGVf",
          "U2VsZWN0Q2xlcmsQAhIjCh9JbmZvTmV3YmllVGFza1R5cGVfUHJvZHVjZUdv",
          "b2RzEAMSIgoeSW5mb05ld2JpZVRhc2tUeXBlX1NlbGVjdEd1ZXN0EAQSJAog",
          "SW5mb05ld2JpZVRhc2tUeXBlX1NwZWNpZmljR3Vlc3QQBRIhCh1JbmZvTmV3",
          "YmllVGFza1R5cGVfR3Vlc3RTcGFjZRAGEiEKHUluZm9OZXdiaWVUYXNrVHlw",
          "ZV9Hb29kc1NwYWNlEAcSIAocSW5mb05ld2JpZVRhc2tUeXBlX0Z1bGxHb29k",
          "cxAIEhoKFkluZm9OZXdiaWVUYXNrVHlwZV9FeHAQCRIeChpJbmZvTmV3Ymll",
          "VGFza1R5cGVfT3BlbkJveBAKEiMKH0luZm9OZXdiaWVUYXNrVHlwZV9VcGdy",
          "YWRlQ2xlcmsQCxIoCiRJbmZvTmV3YmllVGFza1R5cGVfQ2xpY2tQcm9kdWNl",
          "R29vZHMQDBIgChxJbmZvTmV3YmllVGFza1R5cGVfU2FsZUdvb2RzEA0SJAog",
          "SW5mb05ld2JpZVRhc2tUeXBlX1NhbGVUeXBlR29vZHMQDio5CgtDbGVya1N0",
          "YXR1cxIVChFDbGVya1N0YXR1c19VbkdldBAAEhMKD0NsZXJrU3RhdHVzX0dl",
          "dBABYgZwcm90bzM="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(new[] {typeof(global::AssetRef), typeof(global::ErrorCodeType), typeof(global::RewardGetType), typeof(global::NpcType), typeof(global::HomeworldPetStatus), typeof(global::HomeworldStatus), typeof(global::HomeworldFlowerStatus), typeof(global::HomeworldFlowerbedStatus), typeof(global::HomeworldFishType), typeof(global::HomeworldAssetStatus), typeof(global::HomeworldAssetCondition), typeof(global::HomeworldAssetBuffType), typeof(global::InfoBoxStatus), typeof(global::InfoTaskStatus), typeof(global::InfoDailyTaskStatus), typeof(global::InfoShopStatus), typeof(global::InfoDecorateStatus), typeof(global::InfoDecorateType), typeof(global::InfoGoodStatus), typeof(global::IAStatus), typeof(global::IASItemtatus), typeof(global::IASStoreItemStatus), typeof(global::InfoAchCondition), typeof(global::InfoBuff), typeof(global::InfoBuffType), typeof(global::SkillType), typeof(global::SkillEffectType), typeof(global::InfoTimesData), typeof(global::ShopSkillEffectType), typeof(global::InfoNewbieTaskType), typeof(global::ClerkStatus), }, null, null));
  }
  #endregion

}
#region Enums
/// <summary>
/// 资产分类
/// </summary>
public enum AssetRef {
  /// <summary>
  /// 无效
  /// </summary>
  [pbr::OriginalName("ASSET_REF_INVALID")] Invalid = 0,
  /// <summary>
  /// 初始化
  /// </summary>
  [pbr::OriginalName("ASSET_REF_INIT")] Init = 1,
  /// <summary>
  /// GM
  /// </summary>
  [pbr::OriginalName("ASSET_REF_GM")] Gm = 2,
  /// <summary>
  /// 家园钓鱼
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_FISH")] HomeworldFish = 10,
  /// <summary>
  /// 家园收获花-花圃1
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_FLOWER1")] HomeworldFlower1 = 11,
  /// <summary>
  /// 家园收获花-花圃2
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_FLOWER2")] HomeworldFlower2 = 12,
  /// <summary>
  /// 家园收获花-花圃3
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_FLOWER3")] HomeworldFlower3 = 13,
  /// <summary>
  /// 家园收获花-花圃3
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_FLOWER4")] HomeworldFlower4 = 14,
  /// <summary>
  /// 家园卖鱼/花-货架1
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_SALE1")] HomeworldSale1 = 15,
  /// <summary>
  /// 家园卖鱼/花-货架2
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_SALE2")] HomeworldSale2 = 16,
  /// <summary>
  /// 家园卖鱼/花-货架3
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_SALE3")] HomeworldSale3 = 17,
  /// <summary>
  /// 家园Cat卖鱼/花-进猫咪背包
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_CAT_SALE_READY")] HomeworldCatSaleReady = 18,
  /// <summary>
  /// 家园Cat卖鱼/花-领取
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_CAT_SALE")] HomeworldCatSale = 19,
  /// <summary>
  /// 家园卖鱼/花捡起奖励-货架1
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_SALE_PICK1")] HomeworldSalePick1 = 20,
  /// <summary>
  /// 家园卖鱼/花捡起奖励-货架2
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_SALE_PICK2")] HomeworldSalePick2 = 21,
  /// <summary>
  /// 家园卖鱼/花捡起奖励-货架3
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_SALE_PICK3")] HomeworldSalePick3 = 22,
  /// <summary>
  /// 家园未展示奖励
  /// </summary>
  [pbr::OriginalName("ASSET_REF_HOMEWORLD_UNSHOWN")] HomeworldUnshown = 23,
  /// <summary>
  /// 购买员工
  /// </summary>
  [pbr::OriginalName("ASSET_REF_BUY_CLERK")] BuyClerk = 100,
  /// <summary>
  /// 成就奖励员工技能点数
  /// </summary>
  [pbr::OriginalName("ASSET_REF_ACH_REWARD_CLERK_SP")] AchRewardClerkSp = 101,
  /// <summary>
  /// 成就奖励货币
  /// </summary>
  [pbr::OriginalName("ASSET_REF_ACH_REWARD_CURRENCY")] AchRewardCurrency = 102,
  /// <summary>
  /// 小游戏奖励
  /// </summary>
  [pbr::OriginalName("ASSET_REF_VIPGAME")] Vipgame = 103,
  /// <summary>
  /// 商城获得钻石
  /// </summary>
  [pbr::OriginalName("ASSET_REF_STORE_DIAMONDS")] StoreDiamonds = 104,
  /// <summary>
  /// 商城免费礼包获得员工技能点数
  /// </summary>
  [pbr::OriginalName("ASSET_REF_STORE_FREE_GIFT_CLERK_SP")] StoreFreeGiftClerkSp = 105,
  /// <summary>
  /// 商城免费礼包获得货币
  /// </summary>
  [pbr::OriginalName("ASSET_REF_STORE_FREE_GIFT_CURRENCY")] StoreFreeGiftCurrency = 106,
  /// <summary>
  /// 礼品码
  /// </summary>
  [pbr::OriginalName("ASSET_REF_GIFTCODE")] Giftcode = 107,
  /// <summary>
  /// 转盘
  /// </summary>
  [pbr::OriginalName("ASSET_REF_ROULETTE")] Roulette = 108,
}

public enum ErrorCodeType {
  [pbr::OriginalName("ErrorCodeType_Invalid")] Invalid = 0,
  /// <summary>
  /// tip提示
  /// </summary>
  [pbr::OriginalName("ErrorCodeType_Tip")] Tip = 1,
}

public enum RewardGetType {
  [pbr::OriginalName("RewardGetType_Invalid")] Invalid = 0,
  /// <summary>
  /// 普通奖励
  /// </summary>
  [pbr::OriginalName("RewardGetType_Normal")] Normal = 1,
  /// <summary>
  /// 双倍奖励(看广告)
  /// </summary>
  [pbr::OriginalName("RewardGetType_Double")] Double = 2,
  /// <summary>
  /// 1.5被奖励(看广告)
  /// </summary>
  [pbr::OriginalName("RewardGetType_Half")] Half = 3,
}

public enum NpcType {
  [pbr::OriginalName("NpcType_Invalid")] Invalid = 0,
  /// <summary>
  /// 主角
  /// </summary>
  [pbr::OriginalName("NpcType_Hero")] Hero = 1,
  /// <summary>
  /// 安然
  /// </summary>
  [pbr::OriginalName("NpcType_FrontDesk")] FrontDesk = 2,
  /// <summary>
  /// 主角父亲
  /// </summary>
  [pbr::OriginalName("NpcType_HeroFather")] HeroFather = 3,
  /// <summary>
  /// 主角母亲
  /// </summary>
  [pbr::OriginalName("NpcType_HeroMother")] HeroMother = 4,
  /// <summary>
  /// 猫
  /// </summary>
  [pbr::OriginalName("NpcType_Cat")] Cat = 5,
  /// <summary>
  /// 狗
  /// </summary>
  [pbr::OriginalName("NpcType_Dog")] Dog = 6,
  /// <summary>
  /// 羊驼
  /// </summary>
  [pbr::OriginalName("NpcType_Alpaca")] Alpaca = 7,
}

public enum HomeworldPetStatus {
  [pbr::OriginalName("HomeworldPetStatus_Invalid")] Invalid = 0,
  /// <summary>
  /// 可派遣
  /// </summary>
  [pbr::OriginalName("HomeworldPetStatus_Idle")] Idle = 1,
  /// <summary>
  /// 派遣中
  /// </summary>
  [pbr::OriginalName("HomeworldPetStatus_GoOut")] GoOut = 2,
  /// <summary>
  /// 可收获
  /// </summary>
  [pbr::OriginalName("HomeworldPetStatus_GoHome")] GoHome = 3,
  /// <summary>
  /// 冷却中  
  /// </summary>
  [pbr::OriginalName("HomeworldPetStatus_CD")] Cd = 4,
}

public enum HomeworldStatus {
  /// <summary>
  /// 未解锁
  /// </summary>
  [pbr::OriginalName("HomeworldStatus_LOCK")] Lock = 0,
  /// <summary>
  /// 装修倒计时
  /// </summary>
  [pbr::OriginalName("HomeworldStatus_CD")] Cd = 1,
  /// <summary>
  /// 解锁
  /// </summary>
  [pbr::OriginalName("HomeworldStatus_UNLOCK")] Unlock = 2,
}

public enum HomeworldFlowerStatus {
  [pbr::OriginalName("HomeworldFlowerStatus_Invalid")] Invalid = 0,
  /// <summary>
  /// 幼苗
  /// </summary>
  [pbr::OriginalName("HomeworldFlowerStatus_START")] Start = 1,
  /// <summary>
  /// 植株
  /// </summary>
  [pbr::OriginalName("HomeworldFlowerStatus_WORKING")] Working = 2,
  /// <summary>
  /// 开花
  /// </summary>
  [pbr::OriginalName("HomeworldFlowerStatus_DONE")] Done = 3,
}

public enum HomeworldFlowerbedStatus {
  [pbr::OriginalName("HomeworldFlowerbedStatus_Invalid")] Invalid = 0,
  /// <summary>
  /// 可种植
  /// </summary>
  [pbr::OriginalName("HomeworldFlowerbedStatus_EMPTY")] Empty = 1,
  /// <summary>
  /// 种植中
  /// </summary>
  [pbr::OriginalName("HomeworldFlowerbedStatus_WORKING")] Working = 2,
  /// <summary>
  /// 可收获
  /// </summary>
  [pbr::OriginalName("HomeworldFlowerbedStatus_DONE")] Done = 3,
}

public enum HomeworldFishType {
  [pbr::OriginalName("HomeworldFishType_INVALID")] Invalid = 0,
  /// <summary>
  /// 免费次数钓鱼
  /// </summary>
  [pbr::OriginalName("HomeworldFishType_FREE")] Free = 1,
  /// <summary>
  /// 看广告钓鱼
  /// </summary>
  [pbr::OriginalName("HomeworldFishType_AD")] Ad = 2,
}

public enum HomeworldAssetStatus {
  /// <summary>
  /// 资产锁定状态
  /// </summary>
  [pbr::OriginalName("HomeworldAssetStatus_Lock")] Lock = 0,
  /// <summary>
  /// 资产解锁状态
  /// </summary>
  [pbr::OriginalName("HomeworldAssetStatus_Unlock")] Unlock = 1,
  /// <summary>
  /// 资产正在使用状态
  /// </summary>
  [pbr::OriginalName("HomeworldAssetStatus_Using")] Using = 2,
}

public enum HomeworldAssetCondition {
  /// <summary>
  /// 默认
  /// </summary>
  [pbr::OriginalName("HomeworldAssetCondition_Default")] Default = 0,
  /// <summary>
  /// 金币解锁
  /// </summary>
  [pbr::OriginalName("HomeworldAssetCondition_CoinUnlock")] CoinUnlock = 1,
  /// <summary>
  /// 等级解锁
  /// </summary>
  [pbr::OriginalName("HomeworldAssetCondition_GradeUnlock")] GradeUnlock = 2,
  /// <summary>
  /// 登录解锁
  /// </summary>
  [pbr::OriginalName("HomeworldAssetCondition_LoginUnlock")] LoginUnlock = 3,
  /// <summary>
  /// 广告解锁
  /// </summary>
  [pbr::OriginalName("HomeworldAssetCondition_AdUnlock")] AdUnlock = 4,
}

public enum HomeworldAssetBuffType {
  /// <summary>
  /// 默认
  /// </summary>
  [pbr::OriginalName("HABT_Default")] HabtDefault = 0,
  /// <summary>
  /// 花售价提高
  /// </summary>
  [pbr::OriginalName("HABT_FlowerPriceUp")] HabtFlowerPriceUp = 1,
  /// <summary>
  /// 鱼售价提高
  /// </summary>
  [pbr::OriginalName("HABT_FishPriceUp")] HabtFishPriceUp = 2,
  /// <summary>
  /// 花产量提高
  /// </summary>
  [pbr::OriginalName("HABT_FlowerProduceUp")] HabtFlowerProduceUp = 3,
  /// <summary>
  /// 鱼产量提高
  /// </summary>
  [pbr::OriginalName("HABT_FishProduceUp")] HabtFishProduceUp = 4,
  /// <summary>
  /// 花概率额外掉落
  /// </summary>
  [pbr::OriginalName("HABT_FlowerRandomMore")] HabtFlowerRandomMore = 5,
  /// <summary>
  /// 鱼概率额外掉落
  /// </summary>
  [pbr::OriginalName("HABT_FishRandomMore")] HabtFishRandomMore = 6,
  /// <summary>
  /// 宠物带货时间缩短
  /// </summary>
  [pbr::OriginalName("HABT_PetLessSellTime")] HabtPetLessSellTime = 7,
  /// <summary>
  /// 宠物休息时间缩短
  /// </summary>
  [pbr::OriginalName("HABT_PetLessRestTime")] HabtPetLessRestTime = 8,
  /// <summary>
  /// 宠物更多销售经验
  /// </summary>
  [pbr::OriginalName("HABT_PetMoreSellExp")] HabtPetMoreSellExp = 9,
}

public enum InfoBoxStatus {
  /// <summary>
  /// 未开锁
  /// </summary>
  [pbr::OriginalName("InfoBoxStatus_Locked")] Locked = 0,
  /// <summary>
  /// 解锁中
  /// </summary>
  [pbr::OriginalName("InfoBoxStatus_UnLocking")] UnLocking = 1,
  /// <summary>
  /// 已解锁
  /// </summary>
  [pbr::OriginalName("InfoBoxStatus_UnLocked")] UnLocked = 2,
}

public enum InfoTaskStatus {
  /// <summary>
  /// 未开始
  /// </summary>
  [pbr::OriginalName("InfoTaskStatus_UnStart")] UnStart = 0,
  /// <summary>
  /// 进行中
  /// </summary>
  [pbr::OriginalName("InfoTaskStatus_Doing")] Doing = 1,
  /// <summary>
  /// 完成
  /// </summary>
  [pbr::OriginalName("InfoTaskStatus_Done")] Done = 2,
  /// <summary>
  /// 失败
  /// </summary>
  [pbr::OriginalName("InfoTaskStatus_Failure")] Failure = 3,
}

public enum InfoDailyTaskStatus {
  /// <summary>
  /// 未开始
  /// </summary>
  [pbr::OriginalName("InfoDailyTaskStatus_UnStart")] UnStart = 0,
  /// <summary>
  /// 进行中
  /// </summary>
  [pbr::OriginalName("InfoDailyTaskStatus_Doing")] Doing = 1,
  /// <summary>
  /// 待领取前一天奖励
  /// </summary>
  [pbr::OriginalName("InfoDailyTaskStatus_Reward")] Reward = 2,
  /// <summary>
  /// 今日没有挑战次数了
  /// </summary>
  [pbr::OriginalName("InfoDailyTaskStatus_NoTimes")] NoTimes = 3,
}

public enum InfoShopStatus {
  /// <summary>
  /// 未解锁
  /// </summary>
  [pbr::OriginalName("InfoShopStatus_Lock")] Lock = 0,
  /// <summary>
  /// 已解锁
  /// </summary>
  [pbr::OriginalName("InfoShopStatus_Unlock")] Unlock = 1,
  /// <summary>
  /// 正在建造
  /// </summary>
  [pbr::OriginalName("InfoShopStatus_Building")] Building = 2,
  /// <summary>
  /// 已装配
  /// </summary>
  [pbr::OriginalName("InfoShopStatus_Fit")] Fit = 3,
  /// <summary>
  /// 已关闭
  /// </summary>
  [pbr::OriginalName("InfoShopStatus_Close")] Close = 4,
  /// <summary>
  /// 装修中
  /// </summary>
  [pbr::OriginalName("InfoShopStatus_Decorating")] Decorating = 5,
}

public enum InfoDecorateStatus {
  /// <summary>
  ///未解锁
  /// </summary>
  [pbr::OriginalName("InfoDecorateStatus_Lock")] Lock = 0,
  /// <summary>
  ///已解锁，未装修
  /// </summary>
  [pbr::OriginalName("InfoDecorateStatus_Unlock")] Unlock = 1,
  /// <summary>
  ///已装修
  /// </summary>
  [pbr::OriginalName("InfoDecorateStatus_Repair")] Repair = 2,
  /// <summary>
  ///装配中
  /// </summary>
  [pbr::OriginalName("InfoDecorateStatus_Equipped")] Equipped = 3,
  /// <summary>
  ///装修中
  /// </summary>
  [pbr::OriginalName("InfoDecorateStatus_Repairing")] Repairing = 4,
}

public enum InfoDecorateType {
  /// <summary>
  /// 普通装修
  /// </summary>
  [pbr::OriginalName("InfoDecorateType_Normal")] Normal = 0,
  /// <summary>
  /// 立即完成
  /// </summary>
  [pbr::OriginalName("InfoDecorateType_Fast")] Fast = 1,
  /// <summary>
  /// 看广告立即完成
  /// </summary>
  [pbr::OriginalName("InfoDecorateType_AdFast")] AdFast = 2,
  /// <summary>
  /// 装修完成
  /// </summary>
  [pbr::OriginalName("InfoDecorateType_Finish")] Finish = 3,
}

public enum InfoGoodStatus {
  [pbr::OriginalName("InfoGoodStatus_Invalid")] Invalid = 0,
  /// <summary>
  /// 等待生成
  /// </summary>
  [pbr::OriginalName("InfoGoodStatus_Wait")] Wait = 1,
  /// <summary>
  /// 生成中
  /// </summary>
  [pbr::OriginalName("InfoGoodStatus_Working")] Working = 2,
  /// <summary>
  /// 生成完成
  /// </summary>
  [pbr::OriginalName("InfoGoodStatus_Done")] Done = 3,
}

public enum IAStatus {
  /// <summary>
  /// 未解锁
  /// </summary>
  [pbr::OriginalName("IAStatus_Locked")] Locked = 0,
  /// <summary>
  /// 已解锁
  /// </summary>
  [pbr::OriginalName("IAStatus_UnLocked")] UnLocked = 1,
}

public enum IASItemtatus {
  /// <summary>
  /// 已领取奖励
  /// </summary>
  [pbr::OriginalName("IASItemtatus_Reward")] Reward = 0,
  /// <summary>
  /// 进行中
  /// </summary>
  [pbr::OriginalName("IASItemtatus_Doing")] Doing = 1,
  /// <summary>
  /// 已完成，未领取奖励
  /// </summary>
  [pbr::OriginalName("IASItemtatus_Done")] Done = 2,
}

public enum IASStoreItemStatus {
  /// <summary>
  /// 已兑换
  /// </summary>
  [pbr::OriginalName("IASStoreItemStatus_Exchanged")] Exchanged = 0,
  /// <summary>
  /// 成就点不够
  /// </summary>
  [pbr::OriginalName("IASStoreItemStatus_NotEnough")] NotEnough = 1,
  /// <summary>
  /// 兑换CD中，可重复兑换的商品才有兑换CD
  /// </summary>
  [pbr::OriginalName("IASStoreItemStatus_ExchangeCD")] ExchangeCd = 2,
  /// <summary>
  /// 可兑换
  /// </summary>
  [pbr::OriginalName("IASStoreItemStatus_CanExchange")] CanExchange = 3,
  /// <summary>
  /// 可试用
  /// </summary>
  [pbr::OriginalName("IASStoreItemStatus_CanTry")] CanTry = 4,
  /// <summary>
  /// 试用倒计时中
  /// </summary>
  [pbr::OriginalName("IASStoreItemStatus_Trying")] Trying = 5,
}

public enum InfoAchCondition {
  [pbr::OriginalName("InfoAchCondition_None")] None = 0,
  /// <summary>
  /// 指定店铺卖出一定数量指定商品
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Sale")] Sale = 1,
  /// <summary>
  /// 指定员工生成一定数量指定商品
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Produce")] Produce = 2,
  /// <summary>
  /// 拥有存款
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Saving")] Saving = 3,
  /// <summary>
  /// 历史收入
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Income")] Income = 4,
  /// <summary>
  /// 挂机收入
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_OnlineIncome")] OnlineIncome = 5,
  /// <summary>
  /// 澡堂等级
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Level")] Level = 6,
  /// <summary>
  /// 店铺数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_ShopCount")] ShopCount = 7,
  /// <summary>
  /// 店铺等级
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_ShopLv")] ShopLv = 8,
  /// <summary>
  /// 普通任务数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_NormalTaskCount")] NormalTaskCount = 9,
  /// <summary>
  /// 限时任务数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_LimitTaskCount")] LimitTaskCount = 10,
  /// <summary>
  /// 使用体力数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_CostEnergy")] CostEnergy = 11,
  /// <summary>
  /// 招揽顾客数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_NewGuest")] NewGuest = 12,
  /// <summary>
  /// 创造需求数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_NewService")] NewService = 13,
  /// <summary>
  /// 打开宝箱数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_BoxCount")] BoxCount = 14,
  /// <summary>
  /// 获得技能点数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_SPCount")] Spcount = 15,
  /// <summary>
  /// 店员等级
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_ClerkLv")] ClerkLv = 16,
  /// <summary>
  /// 观看广告次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_ADCount")] Adcount = 17,
  /// <summary>
  /// 店员等级达标人数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_ClerkLvCount")] ClerkLvCount = 18,
  /// <summary>
  /// 登录次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_LoginCount")] LoginCount = 19,
  /// <summary>
  /// 在家园中钓到某种鱼的次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Fish")] Fish = 20,
  /// <summary>
  /// 在家园中收获某种花的次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Flower")] Flower = 21,
  /// <summary>
  /// 在家园中卖鱼获得钞票总数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_FishIncome")] FishIncome = 22,
  /// <summary>
  /// 在家园中卖花获得钞票总数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_FlowerIncome")] FlowerIncome = 23,
  /// <summary>
  /// 在家园中派出宠物的次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Pet")] Pet = 24,
  /// <summary>
  /// 房屋装饰数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_House")] House = 25,
  /// <summary>
  /// 拥有汽车的数量
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Car")] Car = 26,
  /// <summary>
  /// 家园钓鱼的等级
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_FishLevel")] FishLevel = 27,
  /// <summary>
  /// 家园花圃的等级
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_FlowerLevel")] FlowerLevel = 28,
  /// <summary>
  /// 玩小游戏的次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_Game")] Game = 29,
  /// <summary>
  /// 在小游戏中完成完美服务的次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_GamePerfect")] GamePerfect = 30,
  /// <summary>
  /// 使用店铺主动技能的次数
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_ShopSkill")] ShopSkill = 31,
  /// <summary>
  /// 获得场景形象
  /// </summary>
  [pbr::OriginalName("InfoAchCondition_SceneDecorate")] SceneDecorate = 32,
}

public enum InfoBuff {
  /// <summary>
  /// 新手任务
  /// </summary>
  [pbr::OriginalName("InfoBuff_NewbieTask")] NewbieTask = 0,
  /// <summary>
  /// 员工技能
  /// </summary>
  [pbr::OriginalName("InfoBuff_ClerkSkill")] ClerkSkill = 1,
  /// <summary>
  /// 店铺技能
  /// </summary>
  [pbr::OriginalName("InfoBuff_ShopSkill")] ShopSkill = 2,
  /// <summary>
  /// 成就
  /// </summary>
  [pbr::OriginalName("InfoBuff_Ach")] Ach = 3,
  /// <summary>
  /// 广告获得
  /// </summary>
  [pbr::OriginalName("InfoBuff_Ad")] Ad = 4,
  /// <summary>
  ///宠物获得
  /// </summary>
  [pbr::OriginalName("InfoBuff_Pet")] Pet = 5,
  /// <summary>
  ///装修获得
  /// </summary>
  [pbr::OriginalName("InfoBuff_Decorate")] Decorate = 6,
}

public enum InfoBuffType {
  [pbr::OriginalName("InfoBuffType_None")] None = 0,
  /// <summary>
  /// 增加体力上限
  /// </summary>
  [pbr::OriginalName("InfoBuffType_EnergyMax")] EnergyMax = 3,
  /// <summary>
  /// 增加客人容量上限
  /// </summary>
  [pbr::OriginalName("InfoBuffType_GuestMax")] GuestMax = 4,
  /// <summary>
  /// 自动补货数量
  /// </summary>
  [pbr::OriginalName("InfoBuffType_AutoGoods")] AutoGoods = 6,
  /// <summary>
  /// 体力恢复速度
  /// </summary>
  [pbr::OriginalName("InfoBuffType_EnergySpeed")] EnergySpeed = 7,
  /// <summary>
  /// 商品生产速度
  /// </summary>
  [pbr::OriginalName("InfoBuffType_ProduceSpeed")] ProduceSpeed = 8,
  /// <summary>
  /// 商品售价提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_PriceUp")] PriceUp = 9,
  /// <summary>
  /// 任务时限提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_TaskTimeUp")] TaskTimeUp = 10,
  /// <summary>
  /// 任务经验提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_TaskExpUp")] TaskExpUp = 11,
  /// <summary>
  /// 宝箱加速
  /// </summary>
  [pbr::OriginalName("InfoBuffType_BoxSpeed")] BoxSpeed = 12,
  /// <summary>
  /// 顾客移动速度提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_GuestMoveSpeed")] GuestMoveSpeed = 13,
  /// <summary>
  /// 购物速度提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_BuySpeed")] BuySpeed = 14,
  /// <summary>
  /// 挂机收益提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_OnlineRewardUp")] OnlineRewardUp = 15,
  /// <summary>
  /// 顾客携带金币提高
  /// </summary>
  [pbr::OriginalName("InfoBuffType_GuestCoinsUp")] GuestCoinsUp = 16,
  /// <summary>
  /// 售货增加经验
  /// </summary>
  [pbr::OriginalName("InfoBuffType_SaleExp")] SaleExp = 17,
  /// <summary>
  /// 免费进货
  /// </summary>
  [pbr::OriginalName("InfoBuffType_FreeProduce")] FreeProduce = 18,
  /// <summary>
  /// 进货额外增加商品数量
  /// </summary>
  [pbr::OriginalName("InfoBuffType_ExtraGoods")] ExtraGoods = 19,
  /// <summary>
  /// 在商店消费，一定概率新增商品需求
  /// </summary>
  [pbr::OriginalName("InfoBuffType_AddGoods")] AddGoods = 20,
}

public enum SkillType {
  [pbr::OriginalName("SkillType_None")] None = 0,
  [pbr::OriginalName("SkillType_Passive")] Passive = 1,
  [pbr::OriginalName("SkillType_Active")] Active = 2,
}

public enum SkillEffectType {
  [pbr::OriginalName("SkillEffectType_None")] None = 0,
  /// <summary>
  /// 增加商品数量
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddGoodsNum")] AddGoodsNum = 1,
  /// <summary>
  /// 增加自动补货数量
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddAutoCount")] AddAutoCount = 2,
  /// <summary>
  /// 增加补货速度
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddProduceSpeed")] AddProduceSpeed = 3,
  /// <summary>
  /// 增加新需求产生概率
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddGoodsProbability")] AddGoodsProbability = 4,
  /// <summary>
  /// 增加售货经验
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddSaleExp")] AddSaleExp = 5,
  /// <summary>
  /// 主动技能-增加商品需求
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddGoods")] AddGoods = 6,
  /// <summary>
  /// 主动技能-加速
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddSpeed")] AddSpeed = 7,
  /// <summary>
  /// 主动技能-加钱
  /// </summary>
  [pbr::OriginalName("SkillEffectType_AddCoins")] AddCoins = 8,
}

public enum InfoTimesData {
  [pbr::OriginalName("InfoTimesData_None")] None = 0,
  /// <summary>
  /// 看广告加速宝箱打开
  /// </summary>
  [pbr::OriginalName("InfoTimesData_OpenBox")] OpenBox = 1,
  /// <summary>
  /// 看广告额外打开宝箱
  /// </summary>
  [pbr::OriginalName("InfoTimesData_ExtraBox")] ExtraBox = 2,
}

public enum ShopSkillEffectType {
  [pbr::OriginalName("ShopSkillEffectType_Invalid")] Invalid = 0,
  /// <summary>
  /// 年中大促
  /// </summary>
  [pbr::OriginalName("ShopSkillEffectType_NZDC")] Nzdc = 1,
  /// <summary>
  /// 土豪撒钱
  /// </summary>
  [pbr::OriginalName("ShopSkillEffectType_THSQ")] Thsq = 2,
}

public enum InfoNewbieTaskType {
  [pbr::OriginalName("InfoNewbieTaskType_None")] None = 0,
  /// <summary>
  /// 解锁店铺
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_UnlockShop")] UnlockShop = 1,
  /// <summary>
  /// 上阵店员
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_SelectClerk")] SelectClerk = 2,
  /// <summary>
  /// 生产商品
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_ProduceGoods")] ProduceGoods = 3,
  /// <summary>
  /// 选择拉客
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_SelectGuest")] SelectGuest = 4,
  /// <summary>
  /// 获得指定需求客人
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_SpecificGuest")] SpecificGuest = 5,
  /// <summary>
  /// 解锁客人位置
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_GuestSpace")] GuestSpace = 6,
  /// <summary>
  /// 解锁备货位置
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_GoodsSpace")] GoodsSpace = 7,
  /// <summary>
  /// 补满店铺商品
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_FullGoods")] FullGoods = 8,
  /// <summary>
  /// 获得澡堂经验
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_Exp")] Exp = 9,
  /// <summary>
  /// 打开宝箱
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_OpenBox")] OpenBox = 10,
  /// <summary>
  /// 升级员工
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_UpgradeClerk")] UpgradeClerk = 11,
  /// <summary>
  /// 点击生产商品
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_ClickProduceGoods")] ClickProduceGoods = 12,
  /// <summary>
  /// 卖出任意商品
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_SaleGoods")] SaleGoods = 13,
  /// <summary>
  /// 卖出指定类型商品
  /// </summary>
  [pbr::OriginalName("InfoNewbieTaskType_SaleTypeGoods")] SaleTypeGoods = 14,
}

public enum ClerkStatus {
  /// <summary>
  /// 未获得
  /// </summary>
  [pbr::OriginalName("ClerkStatus_UnGet")] UnGet = 0,
  /// <summary>
  /// 已获得
  /// </summary>
  [pbr::OriginalName("ClerkStatus_Get")] Get = 1,
}

#endregion


#endregion Designer generated code
